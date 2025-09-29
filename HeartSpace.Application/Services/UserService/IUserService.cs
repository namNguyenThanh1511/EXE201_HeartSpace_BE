using HeartSpace.Application.Services.UserService.DTOs;
using HeartSpace.Domain.Entities;
using HeartSpace.Domain.RequestFeatures;

namespace HeartSpace.Application.Services.UserService
{
    public interface IUserService
    {
        Task<User> FindUserByEmailAsync(string email);
        Task<User> FindUserByUsernameAsync(string username);
        Task SaveUserAsync(User user);
        Task<User> FindUserByPhonenumberAsync(string phonenumber);
        Task<User> FindUserByIdAsync(string id);
        Task UpdateUserAsync(User user);
        Task<UserProfileResponse?> GetUserProfileAsync(Guid userId);
        Task<bool> UpdateUserProfileAsync(Guid userId, UserProfileUpdateDto request);
        Task<PagedList<UserProfileResponse>> GetAllUserProfileAsync(UserQueryParams queryParams);

    }
}
