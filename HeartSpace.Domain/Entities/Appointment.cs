namespace HeartSpace.Domain.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
        public string? Notes { get; set; }//ghi chú cần tư vấn
        public bool IsDeleted { get; set; } = false;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;
        // Navigation properties
        public Guid ScheduleId { get; set; }
        public Schedule? Schedule { get; set; }
        public Guid ClientId { get; set; }
        public User? Client { get; set; }
        public Guid ConsultantId { get; set; }
        public User? Consultant { get; set; }

        public Session? Session { get; set; } // One-to-one relationship
    }

    public enum AppointmentStatus
    {
        Pending,
        Scheduled,
        Completed,
        Cancelled
    }
}
