using HeartSpace.Application.Services.AppointmentService.DTOs;
using HeartSpace.Domain.Entities;

namespace HeartSpace.Application.Services.AppointmentService
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Appointment>> GetAppointmentsByUserIdAsync(Guid userId);
        Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(DateTime date);
        Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync(Guid userId);
        Task<IEnumerable<Appointment>> GetPastAppointmentsAsync(Guid userId);
        Task<AppointmentResponse> CreateAppointmentAsync(AppointmentBookingRequest appointment);
        Task<Appointment> UpdateAppointmentAsync(Appointment appointment);
        Task<bool> DeleteAppointmentAsync(Guid appointmentId);
    }
}
