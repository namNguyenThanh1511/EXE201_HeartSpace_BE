namespace HeartSpace.Infrastructure
{
    using HeartSpace.Domain.Entities;
    using HeartSpace.Infrastructure.Persistence;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public static class DbInitializer
    {
        public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<RepositoryContext>();
            var hasher = new PasswordHasher<User>();

            if (!context.Users.Any())
            {
                var now = DateTimeOffset.UtcNow;

                var admin = new User
                {
                    FullName = "Admin User",
                    Email = "admin@heartspace.com",
                    PhoneNumber = "0900000001",
                    Username = "admin",
                    IsActive = true,
                    UserRole = User.Role.Admin,
                    CreatedAt = now,
                    UpdatedAt = now,
                    DateOfBirth = new DateOnly(1990, 1, 1),
                    Gender = true
                };

                admin.Password = hasher.HashPassword(admin, "123456");

                var consultants = Enumerable.Range(1, 3).Select(i =>
                {
                    var user = new User
                    {
                        FullName = $"Consultant {i}",
                        Email = $"consultant{i}@heartspace.com",
                        PhoneNumber = $"090000000{i + 1}",
                        Username = $"consultant{i}",
                        IsActive = true,
                        UserRole = User.Role.Consultant,
                        CreatedAt = now,
                        UpdatedAt = now,
                        Gender = (i % 2 == 0),
                        DateOfBirth = new DateOnly(1992, i, i + 1)
                    };
                    user.Password = hasher.HashPassword(user, "123456");
                    return user;
                });

                var clients = Enumerable.Range(1, 6).Select(i =>
                {
                    var user = new User
                    {
                        FullName = $"Client {i}",
                        Email = $"client{i}@heartspace.com",
                        PhoneNumber = $"090000001{i + 3}",
                        Username = $"client{i}",
                        IsActive = true,
                        UserRole = User.Role.Client,
                        CreatedAt = now,
                        UpdatedAt = now,
                        Gender = (i % 2 == 1),
                        DateOfBirth = new DateOnly(1995, (i % 12) + 1, (i % 28) + 1)
                    };
                    user.Password = hasher.HashPassword(user, "123456");
                    return user;
                });

                await context.Users.AddAsync(admin);
                await context.Users.AddRangeAsync(consultants);
                await context.Users.AddRangeAsync(clients);
                await context.SaveChangesAsync();
            }
        }
    }

}
