using HeartSpace.Application.Services.TokenService.DTOs;
using HeartSpace.Domain.Entities;

namespace HeartSpace.Application.Services.TokenService
{
    public interface ITokenService
    {
        /// <summary>
        /// Tạo JWT access token
        /// </summary>
        string GenerateAccessToken(User user);

        /// <summary>
        /// Tạo refresh token
        /// </summary>
        Task<string> GenerateRefreshTokenAsync(Guid userId);

        /// <summary>
        /// Validate access token
        /// </summary>
        bool ValidateAccessToken(string token);

        /// <summary>
        /// Validate refresh token
        /// </summary>
        Task<bool> ValidateRefreshTokenAsync(string token);

        Task<TokenResponse> RefreshTokenAsync(string refreshToken);

        /// <summary>
        /// Revoke refresh token
        /// </summary>
        Task RevokeTokenAsync(string token);

        /// <summary>
        /// Revoke tất cả token của user
        /// </summary>
        Task RevokeAllUserTokensAsync(Guid userId);

        /// <summary>
        /// Lấy user từ access token
        /// </summary>
        Task<User?> GetUserFromTokenAsync(string token);

        /// <summary>
        /// Cleanup expired tokens
        /// </summary>
        Task CleanupExpiredTokensAsync();

        /// <summary>
        /// Lấy thông tin từ token
        /// </summary>
        TokenInfo? GetTokenInfo(string token);
    }
}
