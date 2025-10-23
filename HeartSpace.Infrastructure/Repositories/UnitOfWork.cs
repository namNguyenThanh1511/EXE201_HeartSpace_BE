using HeartSpace.Domain.Repositories;
using HeartSpace.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;


namespace HeartSpace.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RepositoryContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(
          RepositoryContext context,
          IUserRepository userRepository,
          IRefreshTokenRepository refreshTokenRepository,
          IScheduleRepository scheduleRepository,
          IAppointmentRepository appointmentRepository,
          IConsultantProfileRepository consultantProfileRepository,
          IConsultantConsultingRepository consultantConsultingRepository,
          IConsultingRepository consultingRepository,
          IPaymentRequestRepository paymentRequestRepository)
        {
            _context = context;
            Users = userRepository;
            RefreshTokens = refreshTokenRepository;
            Schedules = scheduleRepository;
            Appointments = appointmentRepository;
            ConsultantProfiles = consultantProfileRepository;
            ConsultantConsultings = consultantConsultingRepository;
            Consultings = consultingRepository;
            PaymentRequests = paymentRequestRepository;
        }

        public IUserRepository Users { get; }
        public IRefreshTokenRepository RefreshTokens { get; }

        public IScheduleRepository Schedules { get; }

        public IAppointmentRepository Appointments { get; }

        public IConsultantProfileRepository ConsultantProfiles { get; }

        public IConsultantConsultingRepository ConsultantConsultings { get; }
        public IConsultingRepository Consultings { get; }

        public IPaymentRequestRepository PaymentRequests { get; }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("Transaction đã được bắt đầu.");
            }

            _transaction = await _context.Database.BeginTransactionAsync();
            return _transaction;
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("Không có transaction nào để commit.");
            }

            try
            {
                await _transaction.CommitAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("Không có transaction nào để rollback.");
            }

            try
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
