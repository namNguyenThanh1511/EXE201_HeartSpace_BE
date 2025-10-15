using HeartSpace.Api.Models;
using HeartSpace.Application.Services.AuthService;
using HeartSpace.Application.Services.AuthService.DTOs;
using HeartSpace.Application.Services.TokenService;
using HeartSpace.Application.Services.TokenService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeartSpace.Api.Controllers
{
    [Route("api/auth")]
    public class AuthenticationController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        public AuthenticationController(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse>> Register([FromBody] UserCreationDto userCreationDto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError("Dữ liệu không hợp lệ");
            }
            await _authService.RegisterAsync(userCreationDto);
            return Created("Đăng ký tài khoản thành công");
        }

        [HttpPost("register/consultant")]
        public async Task<ActionResult<ApiResponse>> RegisterConsultant([FromBody] RegisterConsultantRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError("Dữ liệu không hợp lệ");
            }
            await _authService.RegisterConsultantAsync(request);
            return Created("Đăng ký tài khoản thành công");
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<TokenResponse>>> Login([FromBody] UserLoginDto userLoginDto)
        {
            // DTO validation tự động bởi Model Binding
            // Nếu validation fail -> BadRequest tự động
            var tokenResponse = await _authService.LoginAsync(userLoginDto);
            return Ok(tokenResponse, "Đăng nhập thành công");

        }

        /// <summary>
        /// Refresh access token using refresh token
        /// </summary>
        /// <param name="request">Refresh token request</param>
        /// <returns>New access token and refresh token</returns>
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<TokenResponse>>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            // Validate request
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return BadRequest<TokenResponse>("Refresh token is required");
            }
            // Call TokenService to refresh token
            var tokenResponse = await _tokenService.RefreshTokenAsync(request.RefreshToken);
            return Ok(tokenResponse, "Token refreshed successfully");
        }
    }
}
