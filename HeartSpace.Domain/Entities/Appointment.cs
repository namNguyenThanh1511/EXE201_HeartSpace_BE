namespace HeartSpace.Domain.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
        public string? Notes { get; set; }//ghi chú cần tư vấn
        public string? ReasonForCancellation { get; set; } // Lý do hủy (nếu có)
        public bool IsDeleted { get; set; } = false;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        // Thêm payment fields
        public string? OrderCode { get; set; }  // Mã thanh toán
        public string? PaymentUrl { get; set; } = null;  // URL thanh toán 
        public decimal Amount { get; set; } = 0;  // Tổng tiền (từ Schedule: 60k hoặc 120k)
        public decimal EscrowAmount { get; set; } = 0;  // Tiền giữ tạm (full amount khi Paid)
        public string? PaymentTransactionId { get; set; }  // ID từ VNPay
        public DateTimeOffset? PaymentDueDate { get; set; }  // Deadline 8h trước StartTime
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.NotPaid;  // Enum mới

        // Navigation properties
        public Guid ScheduleId { get; set; }
        public Schedule? Schedule { get; set; }
        public Guid ClientId { get; set; }
        public User? Client { get; set; }
        public Guid ConsultantId { get; set; }
        public User? Consultant { get; set; }
        public Session? Session { get; set; } // One-to-one relationship

        // Navigation
        public PaymentRequest? PaymentRequest { get; set; }  // Cho rút tiền (one-to-one)
    }

    public enum AppointmentStatus
    {
        Pending,
        PendingPayment,//consultant confirm
        Paid,//client đã thanh toán
        Completed,
        Cancelled
    }

    public enum PaymentStatus
    {
        NotPaid,
        PendingPayment,  // Sau Confirm
        Paid,            // Sau thanh toán thành công
        Refunded,        // Nếu hủy sau Paid
        Withdrawn        // Sau rút hoa hồng
    }
}
