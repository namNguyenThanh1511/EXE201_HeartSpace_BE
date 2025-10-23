namespace HeartSpace.Domain.Entities
{
    // Entity mới: PaymentRequest (cho Consultant rút tiền)
    public class PaymentRequest
    {
        public Guid Id { get; set; }
        public Guid AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
        public decimal RequestAmount { get; set; }  // Hoa hồng 70% (e.g., 42k từ 60k)
        public string BankAccount { get; set; }     // TK ngân hàng Consultant
        public string BankName { get; set; }
        public PaymentRequestStatus Status { get; set; } = PaymentRequestStatus.Pending;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? ProcessedAt { get; set; }
    }
    public enum PaymentRequestStatus
    {
        Pending,
        Approved,
        Rejected,
        Processed  // Đã chuyển khoản
    }
}
