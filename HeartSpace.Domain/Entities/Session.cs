namespace HeartSpace.Domain.Entities
{
    //buổi tư vấn
    public class Session
    {
        public Guid Id { get; set; }
        public string Summary { get; set; }
        public int Rating { get; set; } //đánh giá
        public string Feedback { get; set; } //phản hồi
        public DateTimeOffset EndAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public Guid AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
    }
}
