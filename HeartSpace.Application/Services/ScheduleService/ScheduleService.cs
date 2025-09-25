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
            (string userId, string role) = _currentUserService.GetCurrentUser();
            if (role != Role.Consultant.ToString())
            {
                throw new InsufficientPermissionException("Only Consultant and Admin can create schedules.");
            }
            var schedule = new Schedule
            {
                ConsultantId = Guid.Parse(userId),
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                IsAvailable = true,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            schedule = await _unitOfWork.Schedules.AddAsync(schedule);
            await _unitOfWork.SaveAsync();
            var response = new ScheduleResponse
            {
                Id = schedule.Id,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
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
    }
}
