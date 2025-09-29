using HeartSpace.Application.Services.ScheduleService.DTOs;
using HeartSpace.Domain.Entities;
using HeartSpace.Domain.RequestFeatures;

namespace HeartSpace.Application.Services.ScheduleService
{
    public interface IScheduleService
    {
        Task<IEnumerable<Schedule>> GetAllSchedulesAsync();
        Task<Schedule> GetScheduleByIdAsync(Guid id);
        Task<ScheduleResponse> CreateScheduleAsync(ScheduleCreation schedule);
        Task<Schedule> UpdateScheduleAsync(Domain.Entities.Schedule schedule);
        Task DeleteScheduleAsync(Guid id);

        Task<PagedList<ScheduleResponse>> GetSchedulesByConsultantIdAsync(Guid consultantId, ScheduleQueryParams queryParams);
    }
}
