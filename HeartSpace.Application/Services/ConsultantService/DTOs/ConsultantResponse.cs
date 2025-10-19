using HeartSpace.Application.Services.UserService.DTOs;

namespace HeartSpace.Application.Services.ConsultantService.DTOs
{
    public class ConsultantResponse
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; } = string.Empty;
        public string? Bio { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Username { get; set; } = string.Empty;
        public string? DateOfBirth { get; set; }
        public string? Avatar { get; set; }
        public string? Role { get; set; } = string.Empty;
        public bool? Gender { get; set; }
        public bool? IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public ConsultantDetailResponse? ConsultantInfo { get; set; }
        public List<FreeScheduleResponse>? FreeSchedules { get; set; }
    }
    public class FreeScheduleResponse
    {

        public Guid Id { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public bool IsAvailable { get; set; }
    }

}
