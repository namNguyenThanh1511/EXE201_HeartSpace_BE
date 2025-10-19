using HeartSpace.Domain.Entities;
using HeartSpace.Domain.Repositories;
using HeartSpace.Infrastructure.Persistence;

namespace HeartSpace.Infrastructure.Repositories
{
    public class ConsultingRepository : RepositoryBase<Consulting>, IConsultingRepository
    {
        public ConsultingRepository(RepositoryContext context) : base(context)
        {
        }
    }
}
