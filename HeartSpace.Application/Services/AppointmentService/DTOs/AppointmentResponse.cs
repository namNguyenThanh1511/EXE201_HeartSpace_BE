namespace HeartSpace.Application.Services.AppointmentService.DTOs
{
    public class AppointmentResponse
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; } // Ghi chú cần tư vấn
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public Guid ScheduleId { get; set; }
        public Guid ClientId { get; set; }
        public Guid ConsultantId { get; set; }
        public string? PaymentUrl { get; set; } // URL thanh toán
        public string? PaymentStatus { get; set; } // Trạng thái thanh toán
        public DateTimeOffset? PaymentDueDate { get; set; }

    }
}
