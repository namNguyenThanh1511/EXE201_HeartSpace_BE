using HeartSpace.Domain.Entities;
using HeartSpace.Domain.Repositories;
using HeartSpace.Infrastructure.Persistence;

namespace HeartSpace.Infrastructure.Repositories
{
    public class PaymentRequestRepository : RepositoryBase<PaymentRequest>, IPaymentRequestRepository
    {
        public PaymentRequestRepository(RepositoryContext context) : base(context)
        {
        }

    }
}
