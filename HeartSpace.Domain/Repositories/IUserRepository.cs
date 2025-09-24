
using HeartSpace.Domain.Entities;

namespace HeartSpace.Domain.Repositories
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByUserNameAsync(string userName);
        Task<User?> GetUserByIdentifierAsync(string identifier);
        Task<User?> GetUserByPhoneNumberAsync(string phone);

    }
}
