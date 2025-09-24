using HeartSpace.Domain.Entities;
using HeartSpace.Domain.Repositories;
using HeartSpace.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HeartSpace.Infrastructure.Repositories
{
    public sealed class RefreshTokenRepository : RepositoryBase<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(Guid userId)
        {
            return await _context.RefreshTokens
                .Where(rt => rt.UserId == userId &&
                           !rt.IsRevoked &&
                           rt.ExpiryDate > DateTime.UtcNow)
                .OrderByDescending(rt => rt.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<RefreshToken>> GetAllTokensByUserIdAsync(Guid userId)
        {
            return await _context.RefreshTokens
                .Where(rt => rt.UserId == userId)
                .OrderByDescending(rt => rt.CreatedAt)
            .ToListAsync();
        }

        public async Task RevokeTokenAsync(string token)
        {
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token);

            if (refreshToken != null && !refreshToken.IsRevoked)
            {
                refreshToken.IsRevoked = true;
                refreshToken.RevokedAt = DateTime.UtcNow;
                refreshToken.UpdatedAt = DateTime.UtcNow;

                _context.RefreshTokens.Update(refreshToken);
            }
        }

        public async Task RevokeAllUserTokensAsync(Guid userId)
        {
            var activeTokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && !rt.IsRevoked)
                .ToListAsync();

            foreach (var token in activeTokens)
            {
                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
                token.UpdatedAt = DateTime.UtcNow;
            }

            if (activeTokens.Any())
            {
                _context.RefreshTokens.UpdateRange(activeTokens);
            }
        }

        public async Task RevokeAllUserTokensExceptCurrentAsync(Guid userId, string currentToken)
        {
            var tokensToRevoke = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId &&
                           !rt.IsRevoked &&
                           rt.Token != currentToken)
                .ToListAsync();

            foreach (var token in tokensToRevoke)
            {
                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
                token.UpdatedAt = DateTime.UtcNow;
            }

            if (tokensToRevoke.Any())
            {
                _context.RefreshTokens.UpdateRange(tokensToRevoke);
            }
        }

        public async Task CleanupExpiredTokensAsync()
        {
            var expiredTokens = await _context.RefreshTokens
                .Where(rt => rt.ExpiryDate <= DateTime.UtcNow)
            .ToListAsync();

            if (expiredTokens.Any())
            {
                _context.RefreshTokens.RemoveRange(expiredTokens);
            }
        }

        public async Task<bool> IsTokenValidAsync(string token)
        {
            return await _context.RefreshTokens
                .AnyAsync(rt => rt.Token == token &&
                              !rt.IsRevoked &&
                              rt.ExpiryDate > DateTime.UtcNow);
        }

    }
}
