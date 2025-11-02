using HeartSpace.Application.Services.AppointmentService.DTOs;
using HeartSpace.Domain.Entities;

namespace HeartSpace.Application.Extensions
{
    public static class AppointmentMappingExtension
    {
        // Add mapping methods for Appointment here
        public static AppointmentResponseDetails ToAppointmentResponseDetails(this Appointment appointment)
        {
            return new AppointmentResponseDetails
            {
                Id = appointment.Id,
                Status = appointment.Status.ToString(),
                Notes = appointment.Notes ?? string.Empty,
                CreatedAt = appointment.CreatedAt,
                UpdatedAt = appointment.UpdatedAt,
                ScheduleId = appointment.ScheduleId,
                ClientId = appointment.ClientId,
                ConsultantId = appointment.ConsultantId,
                PaymentUrl = appointment.PaymentUrl,
                PaymentStatus = appointment.PaymentStatus.ToString(),
                PaymentDueDate = appointment.PaymentDueDate,
                ReasonForCancellation = appointment.ReasonForCancellation,
                Amount = appointment.Amount,
                EscrowAmount = appointment.EscrowAmount,
                OrderCode = appointment.OrderCode,
                Schedule = appointment.Schedule != null ? new ScheduleDetails
                {
                    Id = appointment.Schedule.Id,
                    StartTime = appointment.Schedule.StartTime,
                    EndTime = appointment.Schedule.EndTime,
                    Price = appointment.Schedule.Price,
                    IsAvailable = appointment.Schedule.IsAvailable
                } : null,
                Client = appointment.Client != null ? new UserDetails
                {
                    Id = appointment.Client.Id,
                    FullName = appointment.Client.FullName,
                    Email = appointment.Client.Email,
                    PhoneNumber = appointment.Client.PhoneNumber,
                    Avatar = appointment.Client.Avatar
                } : null,
                Consultant = appointment.Consultant != null ? new UserDetails
                {
                    Id = appointment.Consultant.Id,
                    FullName = appointment.Consultant.FullName,
                    Email = appointment.Consultant.Email,
                    PhoneNumber = appointment.Consultant.PhoneNumber,
                    Avatar = appointment.Consultant.Avatar
                } : null,
                Session = appointment.Session != null ? new SessionDetails
                {
                    Id = appointment.Session.Id,
                    Summary = appointment.Session.Summary,
                    Rating = appointment.Session.Rating,
                    Feedback = appointment.Session.Feedback,
                    EndAt = appointment.Session.EndAt
                } : null
            };
        }



    }
}
