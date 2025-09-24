using HeartSpace.Domain.Entities;

namespace HeartSpace.Domain.Repositories
{
    public interface IRefreshTokenRepository : IRepositoryBase<RefreshToken>
    {
        /// <summary>
        /// Lấy refresh token theo token string
        /// </summary>
        Task<RefreshToken?> GetByTokenAsync(string token);

        /// <summary>
        /// Lấy tất cả token active của user
        /// </summary>
        Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(Guid userId);

        /// <summary>
        /// Lấy tất cả token của user (bao gồm revoked)
        /// </summary>
        Task<IEnumerable<RefreshToken>> GetAllTokensByUserIdAsync(Guid userId);

        /// <summary>
        /// Revoke một token cụ thể
        /// </summary>
        Task RevokeTokenAsync(string token);

        /// <summary>
        /// Revoke tất cả token của user
        /// </summary>
        Task RevokeAllUserTokensAsync(Guid userId);

        /// <summary>
        /// Revoke tất cả token của user trừ token hiện tại
        /// </summary>
        Task RevokeAllUserTokensExceptCurrentAsync(Guid userId, string currentToken);

        /// <summary>
        /// Xóa các token đã hết hạn
        /// </summary>
        Task CleanupExpiredTokensAsync();

        /// <summary>
        /// Kiểm tra token có hợp lệ không (chưa revoke và chưa hết hạn)
        /// </summary>
        Task<bool> IsTokenValidAsync(string token);

    }
}
