namespace HeartSpace.Domain.Entities
{
    public class Schedule
    {
        public Guid Id { get; set; }
        public Guid ConsultantId { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public bool IsAvailable { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        // Navigation property
        public User Consultant { get; set; }
        // 1 schedule có thể có nhiều appointment
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
