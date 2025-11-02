namespace HeartSpace.Application.Services.AppointmentService.DTOs
{
    public class AppointmentResponseDetails : AppointmentResponse
    {
        public ScheduleDetails? Schedule { get; set; }
        public UserDetails? Client { get; set; }
        public UserDetails? Consultant { get; set; }
        public SessionDetails? Session { get; set; }
        public string? ReasonForCancellation { get; set; }
        public decimal Amount { get; set; }
        public decimal EscrowAmount { get; set; }
        public long OrderCode { get; set; }
        public string? MeetingLink { get; set; }
    }

    public class ScheduleDetails
    {
        public Guid Id { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class UserDetails
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Avatar { get; set; }
    }

    public class SessionDetails
    {
        public Guid Id { get; set; }
        public string Summary { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Feedback { get; set; } = string.Empty;
        public DateTimeOffset EndAt { get; set; }
    }
}
