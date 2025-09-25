namespace HeartSpace.Application.Services.ScheduleService.DTOs
{
    public class ScheduleResponse
    {
        public Guid Id { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public bool IsAvailable { get; set; }

    }
}
