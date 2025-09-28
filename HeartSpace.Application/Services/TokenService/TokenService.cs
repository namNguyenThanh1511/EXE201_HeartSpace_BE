using HeartSpace.Application.Configuration;
using HeartSpace.Application.Services.TokenService.DTOs;
using HeartSpace.Domain.Entities;
using HeartSpace.Domain.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace HeartSpace.Application.Services.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public TokenService(IOptions<JwtSettings> jwtSettings, IUnitOfWork unitOfWork)
        {
            _jwtSettings = jwtSettings.Value;
            _unitOfWork = unitOfWork;
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public string GenerateAccessToken(User user)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username ?? string.Empty),
                    new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                    new Claim(ClaimTypes.Role, user.UserRole.ToString()),
                    new Claim("FullName", user.FullName ?? string.Empty),
                    new Claim("IsActive", user.IsActive.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat,
                        new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(),
                        ClaimValueTypes.Integer64)
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes * 24),// 60 * 24 = 1 day
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = _tokenHandler.CreateToken(tokenDescriptor);
            return _tokenHandler.WriteToken(token);
        }



        public bool ValidateAccessToken(string token)
        {
            try
            {
                var validationParameters = GetTokenValidationParameters();
                _tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                return validatedToken is JwtSecurityToken jwtToken &&
                       jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> GenerateRefreshTokenAsync(Guid userId)
        {
            var refreshToken = new RefreshToken
            {
                Token = GenerateRandomToken(),
                UserId = userId,
                ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                CreatedAt = DateTime.UtcNow
            };

            _unitOfWork.RefreshTokens.Add(refreshToken);
            await _unitOfWork.SaveAsync();

            return refreshToken.Token;
        }

        public async Task<bool> ValidateRefreshTokenAsync(string token)
        {
            return await _unitOfWork.RefreshTokens.IsTokenValidAsync(token);
        }

        public async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
        {
            var token = await _unitOfWork.RefreshTokens.GetByTokenAsync(refreshToken);

            if (token == null || !token.IsActive)
            {
                throw new UnauthorizedAccessException("Invalid refresh token");
            }

            var user = await _unitOfWork.Users.GetByIdAsync(token.UserId);
            if (user == null || !user.IsActive)
            {
                throw new UnauthorizedAccessException("User not found or inactive");
            }

            // Revoke old refresh token
            await _unitOfWork.RefreshTokens.RevokeTokenAsync(refreshToken);

            // Generate new tokens
            var newAccessToken = GenerateAccessToken(user);
            var newRefreshToken = await GenerateRefreshTokenAsync(user.Id);

            return new TokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                RefreshTokenExpiry = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
            };
        }

        public async Task RevokeTokenAsync(string token)
        {
            await _unitOfWork.RefreshTokens.RevokeTokenAsync(token);
            await _unitOfWork.SaveAsync();
        }

        public async Task RevokeAllUserTokensAsync(Guid userId)
        {
            await _unitOfWork.RefreshTokens.RevokeAllUserTokensAsync(userId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<User?> GetUserFromTokenAsync(string token)
        {
            try
            {
                var validationParameters = GetTokenValidationParameters();
                var principal = _tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                if (validatedToken is JwtSecurityToken jwtToken &&
                    jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
                    if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
                    {
                        return await _unitOfWork.Users.GetByIdAsync(userId);
                    }
                }
            }
            catch
            {
                // Token validation failed
            }

            return null;
        }

        public async Task CleanupExpiredTokensAsync()
        {
            await _unitOfWork.RefreshTokens.CleanupExpiredTokensAsync();
            await _unitOfWork.SaveAsync();
        }

        public TokenInfo? GetTokenInfo(string token)
        {
            try
            {
                var jwtToken = _tokenHandler.ReadJwtToken(token);

                return new TokenInfo
                {
                    UserId = Guid.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value),
                    Username = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? string.Empty,
                    Email = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value ?? string.Empty,
                    Role = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value ?? string.Empty,
                    IssuedAt = jwtToken.ValidFrom,
                    ExpiresAt = jwtToken.ValidTo
                };
            }
            catch
            {
                return null;
            }
        }

        private string GenerateRandomToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        private TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
                ClockSkew = TimeSpan.Zero // Remove default 5 minute tolerance
            };
        }
    }
}
