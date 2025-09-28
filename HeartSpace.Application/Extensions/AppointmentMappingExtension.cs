using HeartSpace.Application.Services.AppointmentService.DTOs;
using HeartSpace.Domain.Entities;

namespace HeartSpace.Application.Extensions
{
    public static class AppointmentMappingExtension
    {
        // Add mapping methods for Appointment here
        public static AppointmentResponse ToAppointmentResponse(this Appointment appointment)
        {
            return new AppointmentResponse
            {
                Id = appointment.Id,
                ClientId = appointment.ClientId,
                ConsultantId = appointment.ConsultantId,
                Status = appointment.Status.ToString(),
                CreatedAt = appointment.CreatedAt,
                UpdatedAt = appointment.UpdatedAt
            };
        }



    }
}
