using HeartSpace.Domain.Entities;
using HeartSpace.Domain.Repositories;
using HeartSpace.Infrastructure.Persistence;

namespace HeartSpace.Infrastructure.Repositories
{
    public class ScheduleRepository : RepositoryBase<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(RepositoryContext context) : base(context)
        {


        }

        public Task<IEnumerable<Schedule>> GetSchedulesByConsultantIdAsync(Guid consultantId)
        {
            var schedules = FindByCondition(s => s.ConsultantId == consultantId).AsEnumerable();
            return Task.FromResult(schedules);
        }
    }
}
