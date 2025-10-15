using Microsoft.EntityFrameworkCore.Storage;

namespace HeartSpace.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IRefreshTokenRepository RefreshTokens { get; }

        IScheduleRepository Schedules { get; }

        IAppointmentRepository Appointments { get; }

        IConsultantProfileRepository ConsultantProfiles { get; }

        IConsultantConsultingRepository ConsultantConsultings { get; }
        // Transaction methods
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task SaveAsync();
        Task SaveAsync(CancellationToken cancellationToken);
    }
}
