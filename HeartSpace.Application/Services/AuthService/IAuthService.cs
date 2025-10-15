using HeartSpace.Application.Services.AuthService.DTOs;
using HeartSpace.Application.Services.TokenService.DTOs;

namespace HeartSpace.Application.Services.AuthService
{
    public interface IAuthService
    {
        Task RegisterAsync(UserCreationDto request);

        Task RegisterConsultantAsync(RegisterConsultantRequest request);

        Task<TokenResponse> LoginAsync(UserLoginDto request);

    }
}
