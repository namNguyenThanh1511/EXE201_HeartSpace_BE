using HeartSpace.Domain.Entities;
using HeartSpace.Domain.Repositories;
using HeartSpace.Infrastructure.Persistence;

namespace HeartSpace.Infrastructure.Repositories
{
    public class ConsultantConsultingRepository : RepositoryBase<ConsultantConsulting>, IConsultantConsultingRepository
    {
        public ConsultantConsultingRepository(RepositoryContext context) : base(context)
        {
        }
    }
}
