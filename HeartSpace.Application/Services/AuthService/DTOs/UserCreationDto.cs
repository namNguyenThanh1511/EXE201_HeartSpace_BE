using HeartSpace.Application.Validators;
using System.ComponentModel.DataAnnotations;

namespace HeartSpace.Application.Services.AuthService.DTOs
{
    public class UserCreationDto
    {
        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(100, ErrorMessage = "Họ tên không được vượt quá 100 ký tự")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [StringLength(255, ErrorMessage = "Email không được vượt quá 255 ký tự")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [VietnamPhoneRegex]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [RegularExpression(@"^[A-Za-z][A-Za-z0-9]{4,}$",
            ErrorMessage = "Tên đăng nhập phải có ít nhất 5 ký tự, chỉ chứa chữ cái và số, bắt đầu bằng chữ cái")]
        public string Username { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateOnly? DateOfBirth { get; set; }

        [RegularExpression(@"^\d{12}$", ErrorMessage = "CMND/CCCD phải có đúng 12 chữ số")]
        public string Identifier { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string Password { get; set; } = string.Empty;
    }
}
