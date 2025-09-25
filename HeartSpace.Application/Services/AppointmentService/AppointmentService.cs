using HeartSpace.Application.Services.AppointmentService.DTOs;
using HeartSpace.Application.Services.UserService;
using HeartSpace.Domain.Entities;
using HeartSpace.Domain.Exception;
using HeartSpace.Domain.Repositories;

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
                Status = AppointmentStatus.Scheduled,
                Notes = request.Notes
            };
            schedule.IsAvailable = false;
            appointment = await _unitOfWork.Appointments.AddAsync(appointment);
            _unitOfWork.Schedules.Update(schedule);
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

        public Task<Appointment> UpdateAppointmentAsync(Appointment appointment)
        {
            throw new NotImplementedException();
        }
    }
}
