using HeartSpace.Application.Services.AppointmentService.DTOs;
using HeartSpace.Domain.Entities;
using HeartSpace.Domain.RequestFeatures;

namespace HeartSpace.Application.Services.AppointmentService
{
    public interface IAppointmentService
    {
        Task<PagedList<AppointmentResponse>> GetAppointmentsAsync(AppointmentQueryParams searchParams);
        Task<IEnumerable<Appointment>> GetAppointmentsByUserIdAsync(Guid userId);
        Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(DateTime date);
        Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync(Guid userId);
        Task<IEnumerable<Appointment>> GetPastAppointmentsAsync(Guid userId);
        Task<AppointmentResponse> CreateAppointmentAsync(AppointmentBookingRequest appointment);
        Task<bool> UpdateAppointmentAsync(Guid id, AppointmentUpdateRequest appointment);
        Task<bool> DeleteAppointmentAsync(Guid appointmentId);
        Task<bool> ProcessPayingAppointment(AppointmentPayingRequest request);
        Task<AppointmentResponseDetails> GetAppointmentByIdAsync(Guid id);
    }
}
