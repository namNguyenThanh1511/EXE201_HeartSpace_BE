
using HeartSpace.Application.Configuration;
using HeartSpace.Application.Extensions;
using HeartSpace.Application.Helpers;
using HeartSpace.Application.Services.AuthService.DTOs;
using HeartSpace.Application.Services.TokenService;
using HeartSpace.Application.Services.TokenService.DTOs;
using HeartSpace.Application.Services.UserService;
using HeartSpace.Domain.Entities;
using HeartSpace.Domain.Exception;
using HeartSpace.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace HeartSpace.Application.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly JwtSettings _jwtSettings;

        public AuthService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher, IUserService userService, ITokenService tokenService, IOptions<JwtSettings> jwtSettings)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _userService = userService;
            _tokenService = tokenService;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<TokenResponse> LoginAsync(UserLoginDto request)
        {
            // Validate and determine login type
            var loginType = LoginTypeHelper.GetLoginType(request.KeyLogin);

            if (loginType == LoginType.Unknown)
            {
                throw new InvalidCredentialsException("Email hoặc số điện thoại không đúng định dạng");
            }

            // Find user based on login type using switch expression
            var user = loginType switch
            {
                LoginType.Email => await _userService.FindUserByEmailAsync(request.KeyLogin),
                LoginType.Phone => await _userService.FindUserByPhonenumberAsync(request.KeyLogin),
                _ => null
            } ?? throw new InvalidCredentialsException("Tài khoản không tồn tại");

            // Check if user is active
            if (!user.IsActive)
            {
                throw new InvalidCredentialsException("Tài khoản đã bị vô hiệu hóa");
            }

            // Verify password
            var verifyResult = _passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);
            if (verifyResult == PasswordVerificationResult.Failed)
            {
                throw new InvalidCredentialsException("Mật khẩu không chính xác");
            }

            // Generate tokens
            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user.Id);

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(GetAccessTokenExpiryMinutes()),
                RefreshTokenExpiry = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
            };
        }

        public async Task RegisterAsync(UserCreationDto userForCreationDto)
        {
            // ✅ Check duplicates BEFORE inserting
            await ValidateUserUniquenessAsync(userForCreationDto);

            //format phone number
            if (!string.IsNullOrEmpty(userForCreationDto.PhoneNumber))
            {
                userForCreationDto.PhoneNumber = PhoneNumberHelper.NormalizePhoneNumber(userForCreationDto.PhoneNumber);
            }

            // Create and save user
            var userEntity = userForCreationDto.MapToUser();
            userEntity.Password = _passwordHasher.HashPassword(userEntity, userForCreationDto.Password);
            userEntity.UserRole = User.Role.User;
            userEntity.IsActive = true;
            userEntity.CreatedAt = DateTimeOffset.UtcNow;
            userEntity.UpdatedAt = DateTimeOffset.UtcNow;

            _unitOfWork.Users.Add(userEntity);
            await _unitOfWork.SaveAsync();
        }

        private async Task ValidateUserUniquenessAsync(UserCreationDto dto)
        {
            // Check email
            var existingUserByEmail = await _unitOfWork.Users.GetUserByEmailAsync(dto.Email);
            if (existingUserByEmail != null)
            {
                throw new UserAlreadyExistsException("email", dto.Email);
            }

            // Check username
            var existingUserByUsername = await _unitOfWork.Users.GetUserByUserNameAsync(dto.Username);
            if (existingUserByUsername != null)
            {
                throw new UserAlreadyExistsException("username", dto.Username);
            }

            // Check identifier
            //var existingUserByIdentifier = await _unitOfWork.Users.GetUserByIdentifierAsync(dto.Identifier);
            //if (existingUserByIdentifier != null)
            //{
            //    throw new UserAlreadyExistsException("identifier", dto.Identifier);
            //}

            // Check phone number (if provided)
            if (!string.IsNullOrEmpty(dto.PhoneNumber))
            {
                var existingUserByPhone = await _unitOfWork.Users.GetUserByPhoneNumberAsync(dto.PhoneNumber);
                if (existingUserByPhone != null)
                {
                    throw new UserAlreadyExistsException("phone number", dto.PhoneNumber);
                }
            }
        }

        private int GetAccessTokenExpiryMinutes()
        {
            return _jwtSettings.AccessTokenExpirationMinutes > 0
                ? _jwtSettings.AccessTokenExpirationMinutes
                : 60; // Default 1 hour
        }


    }
}
