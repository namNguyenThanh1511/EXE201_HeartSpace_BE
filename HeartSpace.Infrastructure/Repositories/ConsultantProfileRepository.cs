using HeartSpace.Domain.Entities;
using HeartSpace.Domain.Repositories;
using HeartSpace.Infrastructure.Persistence;

namespace HeartSpace.Infrastructure.Repositories
{
    public class ConsultantProfileRepository : RepositoryBase<ConsultantProfile>, IConsultantProfileRepository
    {
        public ConsultantProfileRepository(RepositoryContext context) : base(context)
        {
        }
    }
}
