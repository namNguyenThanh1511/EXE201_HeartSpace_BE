namespace HeartSpace.Domain.Entities
{
    public class Schedule
    {
        public Guid Id { get; set; }
        public Guid ConsultantId { get; set; }
        public DateTimeOffset StartTime { get; set; } //mặc định khi tạp slot chỉ chọn khung giờ 30p hoặc 1h
        public DateTimeOffset EndTime { get; set; }
        public decimal Price { get; set; } //60k hoặc 120k
        public bool IsAvailable { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
        // Navigation property
        public User? Consultant { get; set; }
        // 1 schedule có thể có nhiều appointment
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
