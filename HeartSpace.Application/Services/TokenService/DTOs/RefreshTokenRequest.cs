using System.ComponentModel.DataAnnotations;

namespace HeartSpace.Application.Services.TokenService.DTOs
{
    public class RefreshTokenRequest
    {
        [Required(ErrorMessage = "Refresh token is required")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
