using HeartSpace.Application.Services.ScheduleService.DTOs;
using HeartSpace.Application.Services.UserService;
using HeartSpace.Domain.Entities;
using HeartSpace.Domain.Exception;
using HeartSpace.Domain.Repositories;
using static HeartSpace.Domain.Entities.User;

namespace HeartSpace.Application.Services.ScheduleService
{
    public class ScheduleService : IScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public ScheduleService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task<ScheduleResponse> CreateScheduleAsync(ScheduleCreation request)
        {
            if (request.StartTime >= request.EndTime)
                throw new InvalidOperationException("Thời gian bắt đầu phải trước thời gian kết thúc.");

            if ((request.EndTime - request.StartTime).TotalMinutes < 30)
                throw new InvalidOperationException("Lịch phải có thời lượng tối thiểu 30 phút.");

            if (request.StartTime < DateTimeOffset.UtcNow)
                throw new InvalidOperationException("Không thể tạo lịch trong quá khứ.");

            (string userId, string role) = _currentUserService.GetCurrentUser();
            var currentUserSchedules = await _unitOfWork.Schedules.GetSchedulesByConsultantIdAsync(Guid.Parse(userId));
            bool overlaps = currentUserSchedules.Any(s => request.StartTime < s.EndTime && request.EndTime > s.StartTime);
            if (overlaps)
                throw new InvalidOperationException("Lịch mới bị trùng với lịch đã có.");

            if (role != Role.Consultant.ToString())
            {
                throw new InsufficientPermissionException("Only Consultant and Admin can create schedules.");
            }
            var schedule = new Schedule
            {
                ConsultantId = Guid.Parse(userId),
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                IsAvailable = true
            };
            schedule = await _unitOfWork.Schedules.AddAsync(schedule);
            await _unitOfWork.SaveAsync();
            var response = new ScheduleResponse
            {
                Id = schedule.Id,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                IsAvailable = schedule.IsAvailable,
            };
            return response;
        }

        public Task DeleteScheduleAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Schedule>> GetAllSchedulesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Schedule> GetScheduleByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Schedule> UpdateScheduleAsync(Schedule schedule)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<ScheduleResponse>> GetSchedulesByConsultantIdAsync(Guid consultantId)
        {

            var schedules = await _unitOfWork.Schedules.GetSchedulesByConsultantIdAsync(consultantId);
            return schedules.Select(s => new ScheduleResponse
            {
                Id = s.Id,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                IsAvailable = s.IsAvailable
            });
        }
    }
}
