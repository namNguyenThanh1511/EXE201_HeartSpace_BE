namespace HeartSpace.Application.Services.AppointmentService.DTOs
{
    public class AppointmentBookingRequest
    {
        public Guid ScheduleId { get; set; }
        public Guid ClientId { get; set; }
        public string Notes { get; set; } // Ghi chú cần tư vấn
    }
}
