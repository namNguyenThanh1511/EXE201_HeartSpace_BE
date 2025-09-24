using HeartSpace.Domain.Entities;
using HeartSpace.Domain.Repositories;
using HeartSpace.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace HeartSpace.Infrastructure.Repositories
{
    public sealed class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext context) : base(context)
        {
        }
        public async Task<User?> GetUserByEmailAsync(string email)
        => await FindByCondition(u => u.Email.ToLower() == email.ToLower()).SingleOrDefaultAsync();

        public async Task<User?> GetUserByIdAsync(Guid id)
        => await FindByCondition(u => u.Id == id).SingleOrDefaultAsync();

        public async Task<User?> GetUserByIdentifierAsync(string identifier)
        => await FindByCondition(u => u.Identifier == identifier).SingleOrDefaultAsync();

        public async Task<User?> GetUserByUserNameAsync(string userName)
        => await FindByCondition(u => u.Username == userName).SingleOrDefaultAsync();

        public async Task<User?> GetUserByPhoneNumberAsync(string phone)
        => await FindByCondition(u => u.PhoneNumber == phone).SingleOrDefaultAsync();
    }
}
