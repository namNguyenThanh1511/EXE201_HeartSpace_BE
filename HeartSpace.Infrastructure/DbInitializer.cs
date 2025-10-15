namespace HeartSpace.Infrastructure
{
    using HeartSpace.Domain.Entities;
    using HeartSpace.Infrastructure.Persistence;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public static class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<RepositoryContext>();
            var hasher = new PasswordHasher<User>();

            await SeedConsultingsAsync(context);
            await SeedUsersAsync(context, hasher);
        }

        // 🧩 Seed Consulting
        private static async Task SeedConsultingsAsync(RepositoryContext context)
        {
            if (context.Consultings.Any()) return;

            var now = DateTimeOffset.UtcNow;
            var consultings = new List<Consulting>
            {
                new Consulting
                {
                    Name = "Học tập & Định hướng ngành nghề (Study & Career Orientation)",
                    Description = "Giúp học sinh hiểu rõ bản thân, chọn ngành học phù hợp với năng lực và sở thích.",
                    CreateAt = now,
                    UpdatedAt = now
                },
                new Consulting
                {
                    Name = "Chuyển cấp & Thích nghi đại học (Transition & University Adaptation)",
                    Description = "Hướng dẫn sinh viên năm nhất vượt qua khó khăn, xây dựng thói quen học tập và hòa nhập môi trường mới.",
                    CreateAt = now,
                    UpdatedAt = now
                },
                new Consulting
                {
                    Name = "Cảm xúc & Quản lý stress (Emotional Well-being & Stress Management)",
                    Description = "Tư vấn về cân bằng cảm xúc, giải tỏa áp lực, xử lý căng thẳng học đường.",
                    CreateAt = now,
                    UpdatedAt = now
                },
                new Consulting
                {
                    Name = "Phát triển bản thân & Tư duy tích cực (Self-growth & Positive Mindset)",
                    Description = "Giúp xây dựng thái độ sống tích cực, rèn luyện kỹ năng mềm và khám phá tiềm năng cá nhân.",
                    CreateAt = now,
                    UpdatedAt = now
                },
                new Consulting
                {
                    Name = "Định hướng nghề nghiệp & Lộ trình sự nghiệp (Career Direction & Planning)",
                    Description = "Cùng mentor lập kế hoạch nghề nghiệp dài hạn, hướng đến mục tiêu rõ ràng trước và sau khi ra trường.",
                    CreateAt = now,
                    UpdatedAt = now
                },
                new Consulting
                {
                    Name = "Chứng chỉ & Nâng cao năng lực (Certificates & Skill Development)",
                    Description = "Tư vấn chọn chứng chỉ, khóa học kỹ năng (IELTS, MOS, TOEIC, Data, Design...) phù hợp với định hướng.",
                    CreateAt = now,
                    UpdatedAt = now
                },
                new Consulting
                {
                    Name = "Mất định hướng trước khi ra trường (Post-graduation Confusion & Anxiety)",
                    Description = "Hỗ trợ các bạn sinh viên năm cuối vượt qua khủng hoảng 'ra trường rồi làm gì?', định hướng con đường tiếp theo.",
                    CreateAt = now,
                    UpdatedAt = now
                }
            };

            await context.Consultings.AddRangeAsync(consultings);
            await context.SaveChangesAsync();
        }

        // 🧩 Seed User
        private static async Task SeedUsersAsync(RepositoryContext context, PasswordHasher<User> hasher)
        {
            if (context.Users.Any()) return;

            var now = DateTimeOffset.UtcNow;

            var admin = new User
            {
                FullName = "Admin User",
                Email = "admin@heartspace.com",
                PhoneNumber = "0900000001",
                Username = "admin",
                Bio = "Nothing",
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
                    Bio = "Nothing",
                    IsActive = true,
                    UserRole = User.Role.Consultant,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Gender = (i % 2 == 0),
                    DateOfBirth = new DateOnly(1992, i, i + 1)
                };
                user.Password = hasher.HashPassword(user, "123456");
                return user;
            }).ToList();

            var clients = Enumerable.Range(1, 6).Select(i =>
            {
                var user = new User
                {
                    FullName = $"Client {i}",
                    Email = $"client{i}@heartspace.com",
                    PhoneNumber = $"090000001{i + 3}",
                    Username = $"client{i}",
                    Bio = "Nothing",
                    IsActive = true,
                    UserRole = User.Role.Client,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Gender = (i % 2 == 1),
                    DateOfBirth = new DateOnly(1995, (i % 12) + 1, (i % 28) + 1)
                };
                user.Password = hasher.HashPassword(user, "123456");
                return user;
            }).ToList();

            await context.Users.AddAsync(admin);
            await context.Users.AddRangeAsync(consultants);
            await context.Users.AddRangeAsync(clients);
            await context.SaveChangesAsync();
        }
    }
}
