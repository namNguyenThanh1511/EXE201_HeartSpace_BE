namespace HeartSpace.Infrastructure
{
    using HeartSpace.Domain.Entities;
    using HeartSpace.Infrastructure.Persistence;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
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
            await SeedConsultantsAsync(context, hasher);
            await SeedSchedulesAsync(context);

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
                    Name = "Học tập & Định hướng ngành nghề",
                    Description = "Giúp học sinh hiểu rõ bản thân, chọn ngành học phù hợp với năng lực và sở thích.",
                    CreateAt = now,
                    UpdatedAt = now
                },
                new Consulting
                {
                    Name = "Chuyển cấp & Thích nghi đại học",
                    Description = "Hướng dẫn sinh viên năm nhất vượt qua khó khăn, xây dựng thói quen học tập và hòa nhập môi trường mới.",
                    CreateAt = now,
                    UpdatedAt = now
                },
                new Consulting
                {
                    Name = "Cảm xúc & Quản lý stress",
                    Description = "Tư vấn về cân bằng cảm xúc, giải tỏa áp lực, xử lý căng thẳng học đường.",
                    CreateAt = now,
                    UpdatedAt = now
                },
                new Consulting
                {
                    Name = "Phát triển bản thân & Tư duy tích cực",
                    Description = "Giúp xây dựng thái độ sống tích cực, rèn luyện kỹ năng mềm và khám phá tiềm năng cá nhân.",
                    CreateAt = now,
                    UpdatedAt = now
                },
                new Consulting
                {
                    Name = "Định hướng nghề nghiệp & Lộ trình sự nghiệp",
                    Description = "Cùng mentor lập kế hoạch nghề nghiệp dài hạn, hướng đến mục tiêu rõ ràng trước và sau khi ra trường.",
                    CreateAt = now,
                    UpdatedAt = now
                },
                new Consulting
                {
                    Name = "Chứng chỉ & Nâng cao năng lực",
                    Description = "Tư vấn chọn chứng chỉ, khóa học kỹ năng (IELTS, MOS, TOEIC, Data, Design...) phù hợp với định hướng.",
                    CreateAt = now,
                    UpdatedAt = now
                },
                new Consulting
                {
                    Name = "Định hướng trước khi ra trường",
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

            //var consultants = Enumerable.Range(1, 3).Select(i =>
            //{
            //    var user = new User
            //    {
            //        FullName = $"Consultant {i}",
            //        Email = $"consultant{i}@heartspace.com",
            //        PhoneNumber = $"090000000{i + 1}",
            //        Username = $"consultant{i}",
            //        Bio = "Nothing",
            //        IsActive = true,
            //        UserRole = User.Role.Consultant,
            //        CreatedAt = now,
            //        UpdatedAt = now,
            //        Gender = (i % 2 == 0),
            //        DateOfBirth = new DateOnly(1992, i, i + 1)
            //    };
            //    user.Password = hasher.HashPassword(user, "123456");
            //    return user;
            //}).ToList();

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
            //await context.Users.AddRangeAsync(consultants);
            await context.Users.AddRangeAsync(clients);
            await context.SaveChangesAsync();
        }


        private static async Task SeedConsultantsAsync(RepositoryContext context, PasswordHasher<User> hasher)
        {
            if (context.Users.Any(u => u.UserRole == User.Role.Consultant)) return;

            var now = DateTimeOffset.UtcNow;

            // Lấy danh sách Consulting có sẵn
            var consultingIds = context.Consultings.Select(c => c.Id).ToList();
            decimal hourlyRate = 60000;

            // 1️⃣ Consultant 1 - Courtney Henry
            var courtney = new User
            {
                FullName = "Courtney Henry",
                Email = "courtney@example.com",
                PhoneNumber = "0123456789",
                Username = "courtney",
                Bio = "PhD Researcher at the University of Oxford, Environmental Social Sciences.",
                IsActive = true,
                UserRole = User.Role.Consultant,
                CreatedAt = now,
                UpdatedAt = now,
                Gender = false,
                DateOfBirth = new DateOnly(1991, 5, 12),
                Avatar = "https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=150&h=150&fit=crop",
            };
            courtney.Password = hasher.HashPassword(courtney, "123456");
            context.Users.Add(courtney);
            await context.SaveChangesAsync();

            var courtneyProfile = new ConsultantProfile
            {
                ConsultantId = courtney.Id,
                Specialization = "Environmental Studies",
                ExperienceYears = 8,
                HourlyRate = hourlyRate,
                Certifications = "IELTS, MBTI",
                //Rating = 4.9,
                //TotalRatings = 70,
                //Verified = true,

                CreatedAt = now,
                UpdatedAt = now
            };
            context.ConsultantProfiles.Add(courtneyProfile);

            // 2️⃣ Consultant 2 - Jerome Bell
            var jerome = new User
            {
                FullName = "Jerome Bell",
                Email = "jerome@example.com",
                PhoneNumber = "0987654321",
                Username = "jerome",
                Bio = "PhD Researcher at the University of Oxford, Environmental Social Sciences.",
                IsActive = true,
                UserRole = User.Role.Consultant,
                CreatedAt = now,
                UpdatedAt = now,
                Gender = true,
                DateOfBirth = new DateOnly(1988, 3, 25),
                Avatar = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=150&h=150&fit=crop",
            };
            jerome.Password = hasher.HashPassword(jerome, "123456");
            context.Users.Add(jerome);
            await context.SaveChangesAsync();

            var jeromeProfile = new ConsultantProfile
            {
                ConsultantId = jerome.Id,
                Specialization = "Process Engineering",
                ExperienceYears = 12,
                HourlyRate = hourlyRate,
                Certifications = "TOEIC, Process Engineering",
                //Rating = 4.9,
                //TotalRatings = 70,
                //Verified = true,

                CreatedAt = now,
                UpdatedAt = now
            };
            context.ConsultantProfiles.Add(jeromeProfile);

            // 3️⃣ Consultant 3 - Jenny Wilson
            var jenny = new User
            {
                FullName = "Jenny Wilson",
                Email = "jenny@example.com",
                PhoneNumber = "0123456788",
                Username = "jenny",
                Bio = "Carbon Reduction Expert.",
                IsActive = true,
                UserRole = User.Role.Consultant,
                CreatedAt = now,
                UpdatedAt = now,
                Gender = false,
                DateOfBirth = new DateOnly(1993, 7, 4),
                Avatar = "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=150&h=150&fit=crop",
            };
            jenny.Password = hasher.HashPassword(jenny, "123456");
            context.Users.Add(jenny);
            await context.SaveChangesAsync();

            var jennyProfile = new ConsultantProfile
            {
                ConsultantId = jenny.Id,
                Specialization = "Carbon Reduction",
                ExperienceYears = 6,
                HourlyRate = hourlyRate,
                Certifications = "Environmental Leadership, Soft Skills",
                //Rating = 4.9,
                //TotalRatings = 70,
                //Verified = true,

                CreatedAt = now,
                UpdatedAt = now
            };
            context.ConsultantProfiles.Add(jennyProfile);

            // 4️⃣ Consultant 4 - Cameron Williamson
            var cameron = new User
            {
                FullName = "Cameron Williamson",
                Email = "cameron@example.com",
                PhoneNumber = "0987654320",
                Username = "cameron",
                Bio = "Biodiversity Specialist - Developer.",
                IsActive = true,
                UserRole = User.Role.Consultant,
                CreatedAt = now,
                UpdatedAt = now,
                Gender = true,
                DateOfBirth = new DateOnly(1990, 11, 15),
                Avatar = "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=150&h=150&fit=crop",
            };
            cameron.Password = hasher.HashPassword(cameron, "123456");
            context.Users.Add(cameron);
            await context.SaveChangesAsync();

            var cameronProfile = new ConsultantProfile
            {
                ConsultantId = cameron.Id,
                Specialization = "Biodiversity",
                ExperienceYears = 10,
                HourlyRate = hourlyRate,
                Certifications = "Systems Admin, Process Engineer",
                //Rating = 4.9,
                //TotalRatings = 70,
                //Verified = false,

                CreatedAt = now,
                UpdatedAt = now
            };
            context.ConsultantProfiles.Add(cameronProfile);

            // 5️⃣ Gắn lĩnh vực tư vấn (ConsultantConsulting) cho từng người
            var courtneyConsultings = context.Consultings.Where(c => new[] { "1", "2", "3" }.Contains(c.Id.ToString()))
                .Select(c => new ConsultantConsulting { ConsultantId = courtney.Id, ConsultingId = c.Id }).ToList();

            var jeromeConsultings = context.Consultings.Where(c => new[] { "4", "5" }.Contains(c.Id.ToString()))
                .Select(c => new ConsultantConsulting { ConsultantId = jerome.Id, ConsultingId = c.Id }).ToList();

            var jennyConsultings = context.Consultings.Where(c => new[] { "6", "2" }.Contains(c.Id.ToString()))
                .Select(c => new ConsultantConsulting { ConsultantId = jenny.Id, ConsultingId = c.Id }).ToList();

            var cameronConsultings = context.Consultings.Where(c => new[] { "7", "4" }.Contains(c.Id.ToString()))
                .Select(c => new ConsultantConsulting { ConsultantId = cameron.Id, ConsultingId = c.Id }).ToList();

            context.ConsultantConsultings.AddRange(courtneyConsultings);
            context.ConsultantConsultings.AddRange(jeromeConsultings);
            context.ConsultantConsultings.AddRange(jennyConsultings);
            context.ConsultantConsultings.AddRange(cameronConsultings);

            await context.SaveChangesAsync();
        }

        private static async Task SeedSchedulesAsync(RepositoryContext context)
        {
            if (context.Schedules.Any()) return;

            var future = DateTimeOffset.UtcNow.AddDays(1);

            // Lấy danh sách consultants đã được seed
            var consultants = await context.Users
                .Where(u => u.UserRole == User.Role.Consultant)
                .ToListAsync();

            // Các độ dài lịch xen kẽ 30 và 60 phút
            var durations = new[] { 30, 60 };
            // Offset để tránh trùng lặp thời gian (dù khác consultant nhưng để lịch rõ ràng hơn)
            var offsets = new[] { 0, 40, 110, 170 }; // phút

            int index = 0;
            foreach (var consultant in consultants)
            {
                var duration = durations[index % durations.Length];
                var startTime = future.AddMinutes(offsets[index]);
                var endTime = startTime.AddMinutes(duration);
                var price = duration switch
                {
                    30 => 60000,
                    60 => 120000,
                    _ => 0 // Không xảy ra
                };

                var schedule = new Schedule
                {
                    ConsultantId = consultant.Id,
                    StartTime = startTime,
                    EndTime = endTime,
                    Price = price,
                    IsAvailable = true
                };

                context.Schedules.Add(schedule);
                index++;
            }

            await context.SaveChangesAsync();
        }


    }
}
