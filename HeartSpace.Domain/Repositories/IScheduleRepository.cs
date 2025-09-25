using HeartSpace.Domain.Entities;

namespace HeartSpace.Domain.Repositories
{
    public interface IScheduleRepository : IRepositoryBase<Schedule>
    {
        Task<IEnumerable<Schedule>> GetSchedulesByConsultantIdAsync(Guid consultantId);
    }
}
