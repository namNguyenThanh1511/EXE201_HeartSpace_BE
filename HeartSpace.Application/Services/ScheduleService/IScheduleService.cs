using HeartSpace.Application.Services.ScheduleService.DTOs;

namespace HeartSpace.Application.Services.ScheduleService
{
    public interface IScheduleService
    {
        Task<IEnumerable<Domain.Entities.Schedule>> GetAllSchedulesAsync();
        Task<Domain.Entities.Schedule> GetScheduleByIdAsync(Guid id);
        Task<ScheduleResponse> CreateScheduleAsync(ScheduleCreation schedule);
        Task<Domain.Entities.Schedule> UpdateScheduleAsync(Domain.Entities.Schedule schedule);
        Task DeleteScheduleAsync(Guid id);

        Task<IEnumerable<ScheduleResponse>> GetSchedulesByConsultantIdAsync(Guid consultantId);
    }
}
