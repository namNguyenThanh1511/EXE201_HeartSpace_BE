using HeartSpace.Domain.Entities;

namespace HeartSpace.Domain.Repositories
{
    public interface IAppointmentRepository : IRepositoryBase<Appointment>
    {
        Task<IEnumerable<Appointment>> GetAppointmentsByUserIdAsync(Guid userId);
        Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(DateTime date);
        Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync(Guid userId);
        Task<IEnumerable<Appointment>> GetPastAppointmentsAsync(Guid userId);
        Task<Appointment?> GetByIdWithScheduleAsync(Guid id);
    }
}
