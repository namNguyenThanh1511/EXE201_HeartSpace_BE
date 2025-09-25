using HeartSpace.Application.Validators;
using System.ComponentModel.DataAnnotations;

namespace HeartSpace.Application.Services.UserService.DTOs
{
    public class UserProfileUpdateDto
    {
        public string? FullName { get; set; }

        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string? Email { get; set; }

        [VietnamPhoneRegex]
        public string? PhoneNumber { get; set; }

        [RegularExpression(@"^[A-Za-z][A-Za-z0-9]{4,}$",
            ErrorMessage = "Tên đăng nhập phải có ít nhất 5 ký tự, chỉ chứa chữ cái và số, bắt đầu bằng chữ cái")]
        public string? Username { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? DateOfBirth { get; set; }

        [RegularExpression(@"^\d{12}$", ErrorMessage = "CMND/CCCD phải có đúng 12 chữ số")]
        public string? Identifier { get; set; }

        public string? Avatar { get; set; }

        public bool? Gender { get; set; }
    }
}
