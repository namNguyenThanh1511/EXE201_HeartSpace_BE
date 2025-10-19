namespace HeartSpace.Application.Services.UserService.DTOs
{
    public class UserProfileResponse
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; } = string.Empty;
        public string? Bio { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? UserName { get; set; } = string.Empty;
        public string? DateOfBirth { get; set; }
        public string? Identifier { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public string? Role { get; set; } = string.Empty;
        public bool? Gender { get; set; }
        public bool? IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        // Computed properties
        public int? Age { get; set; }
        public bool IsAdult { get; set; }
        public string? AvatarUrl { get; set; } = string.Empty;

        public ConsultantDetailResponse? ConsultantInfo { get; set; }
    }
}
