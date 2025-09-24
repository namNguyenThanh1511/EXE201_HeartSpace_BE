using static HeartSpace.Domain.Entities.User;

namespace HeartSpace.Application.Services.UserService.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? Avatar { get; set; }
        public string? Identifier { get; set; }
        public Role? UserRole { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

    }
}
