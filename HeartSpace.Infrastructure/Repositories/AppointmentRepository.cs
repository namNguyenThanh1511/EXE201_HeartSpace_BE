using HeartSpace.Domain.Entities;
using HeartSpace.Domain.Repositories;
using HeartSpace.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HeartSpace.Infrastructure.Repositories
{
    public class AppointmentRepository : RepositoryBase<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(RepositoryContext context) : base(context)
        {
        }
        public Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Appointment>> GetAppointmentsByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Appointment>> GetPastAppointmentsAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Appointment?> GetByIdWithScheduleAsync(Guid id)
        {
            return await _context.Appointments
                .Include(a => a.Schedule)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Appointment?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Appointments
                .Include(a => a.Schedule)
                .Include(a => a.Client)
                .Include(a => a.Consultant)
                .Include(a => a.Session)
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
        }
    }
}
