using HeartSpace.Application.Extensions;
using HeartSpace.Application.Services.AppointmentService.DTOs;
using HeartSpace.Application.Services.UserService;
using HeartSpace.Domain.Entities;
using HeartSpace.Domain.Exception;
using HeartSpace.Domain.Repositories;
using HeartSpace.Domain.RequestFeatures;

namespace HeartSpace.Application.Services.AppointmentService
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        public AppointmentService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<PagedList<AppointmentResponse>> GetAppointmentsAsync(AppointmentQueryParams searchParams)
        {
            // 1. Lấy thông tin người dùng hiện tại
            (string userId, string role) = _currentUserService.GetCurrentUser();

            // 2. Áp filter dựa theo vai trò
            switch (role)
            {
                case var r when r == User.Role.Admin.ToString():
                    // Admin có thể xem tất cả => không thêm filter
                    break;

                case var r when r == User.Role.Consultant.ToString():
                    searchParams.ConsultantId = Guid.Parse(userId);
                    break;

                case var r when r == User.Role.Client.ToString():
                    searchParams.ClientId = Guid.Parse(userId);
                    break;

                default:
                    throw new InsufficientPermissionException("Bạn không có quyền xem lịch hẹn.");
            }

            // 3. Tạo query cơ sở
            IQueryable<Appointment> query = _unitOfWork.Appointments.FindByCondition((a => !a.IsDeleted));

            // 4. Áp các filter khác từ searchParams
            if (searchParams.ClientId.HasValue)
                query = query.Where(a => a.ClientId == searchParams.ClientId.Value);

            if (searchParams.ConsultantId.HasValue)
                query = query.Where(a => a.ConsultantId == searchParams.ConsultantId.Value);

            if (searchParams.StartDate.HasValue)
                query = query.Where(a => a.Schedule.StartTime >= searchParams.StartDate.Value);

            if (searchParams.EndDate.HasValue)
                query = query.Where(a => a.Schedule.EndTime <= searchParams.EndDate.Value);

            if (searchParams.Status.HasValue)
                query = query.Where(a => a.Status == searchParams.Status.Value);

            // 5. Include navigation properties nếu cần
            //query = query
            //    .Include(a => a.Client)
            //    .Include(a => a.Consultant)
            //    .Include(a => a.Schedule);

            // 6. Thực hiện phân trang
            var pagedAppointments = await PagedList<Appointment>.ToPagedList(
                query.OrderByDescending(a => a.CreatedAt),
                searchParams.PageNumber,
                searchParams.PageSize
            );

            // 7. Map sang DTO (AppointmentResponse)
            var result = pagedAppointments.Select(a => a.ToAppointmentResponse()).ToList();

            return new PagedList<AppointmentResponse>(result, pagedAppointments.Count,
                searchParams.PageNumber, searchParams.PageSize);
        }




        public async Task<AppointmentResponse> CreateAppointmentAsync(AppointmentBookingRequest request)
        {
            var schedule = await _unitOfWork.Schedules.GetByIdAsync(request.ScheduleId) ?? throw new EntityNotFoundException("Không tìm thấy lịch phù hợp");
            if (!schedule.IsAvailable)
                throw new BusinessRuleViolationException("Lịch đã được đặt trước đó.");
            (string userId, string role) = _currentUserService.GetCurrentUser();
            if (role != User.Role.Client.ToString())
                throw new InsufficientPermissionException("Chỉ có khách hàng mới có thể đặt lịch hẹn.");
            var appointment = new Appointment
            {
                ScheduleId = request.ScheduleId,
                ClientId = Guid.Parse(userId),
                ConsultantId = schedule.ConsultantId,
                Status = AppointmentStatus.Pending,
                Notes = request.Notes
            };
            //schedule.IsAvailable = false;
            appointment = await _unitOfWork.Appointments.AddAsync(appointment);
            //_unitOfWork.Schedules.Update(schedule);
            await _unitOfWork.SaveAsync();
            var response = new AppointmentResponse
            {
                Id = appointment.Id,
                Status = appointment.Status.ToString(),
                Notes = appointment.Notes ?? string.Empty,
                CreatedAt = appointment.CreatedAt,
                UpdatedAt = appointment.UpdatedAt,
                ScheduleId = appointment.ScheduleId,
                ClientId = appointment.ClientId,
                ConsultantId = appointment.ConsultantId
            };
            return response;
        }

        public Task<bool> DeleteAppointmentAsync(Guid appointmentId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Appointment>> GetAppointmentsByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Appointment>> GetPastAppointmentsAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAppointmentAsync(Guid id, AppointmentUpdateRequest request)
        {
            var appointment = _unitOfWork.Appointments.GetByIdWithScheduleAsync(id).Result ?? throw new EntityNotFoundException("Không tìm thấy lịch hẹn phù hợp");
            (string userId, string role) = _currentUserService.GetCurrentUser();
            var isOwner = appointment.ClientId == Guid.Parse(userId) || appointment.ConsultantId == Guid.Parse(userId);
            if (!isOwner && role != User.Role.Admin.ToString())
                throw new InsufficientPermissionException("Chỉ có khách hàng, tư vấn  hoặc quản trị viên mới có thể cập nhật lịch hẹn.");
            switch (request.For)
            {
                case AppointmentUpdateRequest.UpdateFor.ConfirmAppointment:
                    if (role != User.Role.Consultant.ToString() && role != User.Role.Admin.ToString())
                        throw new InsufficientPermissionException("Chỉ có tư vấn hoặc quản trị viên mới có thể chấp nhận yêu cầu lịch hẹn.");
                    await ConfirmAppointmentAsync(appointment);
                    return true;
                case AppointmentUpdateRequest.UpdateFor.CancelAppointment:
                    if (string.IsNullOrWhiteSpace(request.ReasonForCancellation))
                        throw new BusinessRuleViolationException("Lý do hủy không được để trống.");
                    await CancelAppointmentAsync(appointment, request.ReasonForCancellation);
                    return true;
                case AppointmentUpdateRequest.UpdateFor.CompleteAppointment:
                    await CompleteAppointmentAsync(appointment);
                    return true;
                case AppointmentUpdateRequest.UpdateFor.RescheduleAppointment:
                    await RescheduleAppointmentAsync(appointment, request.NewScheduleId);
                    return true;
                case AppointmentUpdateRequest.UpdateFor.AddNotes:
                    if (string.IsNullOrWhiteSpace(request.Notes))
                        throw new BusinessRuleViolationException("Ghi chú không được để trống.");
                    appointment.Notes = request.Notes;
                    appointment.UpdatedAt = DateTimeOffset.UtcNow;
                    _unitOfWork.Appointments.Update(appointment);
                    await _unitOfWork.SaveAsync();
                    return true;
                default:
                    throw new BusinessRuleViolationException("Yêu cầu cập nhật không hợp lệ.");
            }



        }

        private async Task ConfirmAppointmentAsync(Appointment appointment)
        {
            if (appointment.Status != AppointmentStatus.Pending)
                throw new BusinessRuleViolationException("Chỉ có thể chấp nhận yêu cầu lịch hẹn đang chờ");

            appointment.Status = AppointmentStatus.Confirm;
            appointment.UpdatedAt = DateTimeOffset.UtcNow;
            _unitOfWork.Appointments.Update(appointment);

            appointment.Schedule.IsAvailable = false;
            _unitOfWork.Schedules.Update(appointment.Schedule);
            await _unitOfWork.SaveAsync();
        }
        private async Task CancelAppointmentAsync(Appointment appointment, string reason)
        {
            // Kiểm tra trạng thái
            if (appointment.Status != AppointmentStatus.Pending &&
                appointment.Status != AppointmentStatus.Confirm)
                throw new BusinessRuleViolationException("Chỉ có thể hủy lịch hẹn đang chờ hoặc đã xác nhận");


            // Tính khoảng cách thời gian giữa hiện tại và thời gian bắt đầu
            var now = DateTimeOffset.UtcNow;
            var diffHours = (appointment.Schedule.StartTime - now).TotalHours;

            // Kiểm tra điều kiện 8 tiếng
            if (diffHours < 8)
                throw new BusinessRuleViolationException("Chỉ có thể hủy lịch hẹn trước ít nhất 8 tiếng");

            // Cho phép hủy
            appointment.Status = AppointmentStatus.Cancelled;
            appointment.ReasonForCancellation = reason;
            appointment.UpdatedAt = now;

            // Cho phép schedule được đặt lại
            appointment.Schedule.IsAvailable = true;
            appointment.Schedule.UpdatedAt = now;

            // Update & Save
            _unitOfWork.Appointments.Update(appointment);
            _unitOfWork.Schedules.Update(appointment.Schedule);
            await _unitOfWork.SaveAsync();
        }


        private async Task CompleteAppointmentAsync(Appointment appointment)
        {
            if (appointment.Status != AppointmentStatus.Confirm)
                throw new BusinessRuleViolationException("Chỉ có thể hoàn thành lịch hẹn đã được xác nhận.");
            if (appointment.Schedule != null && appointment.Schedule.EndTime > DateTimeOffset.UtcNow)
                throw new BusinessRuleViolationException("Chỉ có thể hoàn thành lịch hẹn sau khi thời gian kết thúc.");
            if (appointment.Status == AppointmentStatus.Completed)
                throw new BusinessRuleViolationException("Lịch hẹn đã được hoàn thành trước đó.");
            if (appointment.Status == AppointmentStatus.Cancelled)
                throw new BusinessRuleViolationException("Lịch hẹn đã bị hủy, không thể hoàn thành.");
            if (appointment.Status == AppointmentStatus.Pending)
                throw new BusinessRuleViolationException("Lịch hẹn đang chờ, không thể hoàn thành.");

            appointment.Status = AppointmentStatus.Completed;
            appointment.UpdatedAt = DateTimeOffset.UtcNow;
            _unitOfWork.Appointments.Update(appointment);
            await _unitOfWork.SaveAsync();
        }

        private async Task RescheduleAppointmentAsync(Appointment appointment, Guid? newScheduleId)
        {
            var newSchedule = await _unitOfWork.Schedules.GetByIdAsync(newScheduleId) ?? throw new EntityNotFoundException("Không tìm thấy lịch phù hợp");
            if (!newSchedule.IsAvailable)
                throw new BusinessRuleViolationException("Lịch mới đã được đặt trước đó.");
            if (appointment.Status == AppointmentStatus.Completed)
                throw new BusinessRuleViolationException("Lịch hẹn đã hoàn thành, không thể thay đổi.");
            if (appointment.Status == AppointmentStatus.Cancelled)
                throw new BusinessRuleViolationException("Lịch hẹn đã bị hủy, không thể thay đổi.");
            if (appointment.Status == AppointmentStatus.Confirm && appointment.Schedule != null && appointment.Schedule.StartTime <= DateTimeOffset.UtcNow && appointment.Schedule.StartTime.Hour - DateTimeOffset.UtcNow.Hour < 8)
                throw new BusinessRuleViolationException("Chỉ có thể thay đổi lịch hẹn đã được xác nhận trước thời gian bắt đầu 8 tiếng");
            if (appointment.Status == AppointmentStatus.Confirm)
            {
                var oldSchedule = appointment.Schedule;
                oldSchedule.IsAvailable = true;
                _unitOfWork.Schedules.Update(oldSchedule);
            }
            newSchedule.IsAvailable = false;
            appointment.ScheduleId = newScheduleId.Value;
            appointment.Status = AppointmentStatus.Pending;
            appointment.UpdatedAt = DateTimeOffset.UtcNow;
            _unitOfWork.Schedules.Update(newSchedule);
            _unitOfWork.Appointments.Update(appointment);
            await _unitOfWork.SaveAsync();
        }

        private bool IsUserOwnerOfAppointment(Appointment appointment, string userId)
        {
            return appointment.ClientId == Guid.Parse(userId) || appointment.ConsultantId == Guid.Parse(userId);
        }
        private bool IsValidStatusTransition(AppointmentStatus currentStatus, AppointmentStatus newStatus)
        {
            //need throw exception with message
            return currentStatus switch
            {
                AppointmentStatus.Pending => newStatus == AppointmentStatus.Confirm || newStatus == AppointmentStatus.Cancelled,
                AppointmentStatus.Confirm => newStatus == AppointmentStatus.Completed || newStatus == AppointmentStatus.Cancelled,
                AppointmentStatus.Completed => false,
                AppointmentStatus.Cancelled => false,
                _ => false,
            };
        }


    }
}
