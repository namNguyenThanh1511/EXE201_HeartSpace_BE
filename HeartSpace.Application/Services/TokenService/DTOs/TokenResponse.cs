namespace HeartSpace.Application.Services.TokenService.DTOs
{
    public class TokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime AccessTokenExpiry { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
        public string TokenType { get; set; } = "Bearer";
    }
}
