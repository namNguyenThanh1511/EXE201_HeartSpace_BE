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
            await SeedAppointmentsAsync(context);

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
            decimal hourlyRate = 20000; // sinh viên mới ra trường, phí thấp

            // Lấy danh sách Consulting có sẵn
            var consultingIds = context.Consultings.Select(c => c.Id).ToList();

            // 1️⃣ Consultant 1 - Nguyễn Đường Tiểu My (female)
            var duyen = new User
            {
                FullName = "Nguyễn Đường Tiểu My",
                Email = "myndtse182502@fpt.edu.vn",
                PhoneNumber = "0123456789",
                Username = "duyen",
                Bio = "Sinh viên mới ra trường, có thể tư vấn học tập, định hướng ngành nghề và phát triển bản thân.",
                IsActive = true,
                UserRole = User.Role.Consultant,
                CreatedAt = now,
                UpdatedAt = now,
                Gender = false,
                DateOfBirth = new DateOnly(2001, 5, 12),
                MeetingLink = "https://meet.google.com/mhh-utmy-kgr",
                Avatar = "https://i.postimg.cc/KYM7Cn4c/My.avif?w=150&h=150&fit=crop",
            };
            duyen.Password = hasher.HashPassword(duyen, "123456");
            context.Users.Add(duyen);
            await context.SaveChangesAsync();

            var duyenProfile = new ConsultantProfile
            {
                ConsultantId = duyen.Id,
                Specialization = "Học tập & Định hướng ngành nghề",
                ExperienceYears = 1,
                HourlyRate = hourlyRate,
                Certifications = "Chưa có chứng chỉ chuyên môn",
                CreatedAt = now,
                UpdatedAt = now
            };
            context.ConsultantProfiles.Add(duyenProfile);

            // 2️⃣ Consultant 2 - Vũ Bảo (male)
            var baovinh = new User
            {
                FullName = "Vũ Bảo",
                Email = "baovse150649@fpt.edu.vn",
                PhoneNumber = "0987654321",
                Username = "baovinh",
                Bio = "Sinh viên mới ra trường, có thể tư vấn chuyển cấp, thích nghi đại học và quản lý stress.",
                IsActive = true,
                UserRole = User.Role.Consultant,
                CreatedAt = now,
                UpdatedAt = now,
                Gender = true,
                DateOfBirth = new DateOnly(2000, 3, 25),
                MeetingLink = "https://meet.google.com/ipp-bafa-cux",
                Avatar = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=150&h=150&fit=crop",
            };
            baovinh.Password = hasher.HashPassword(baovinh, "123456");
            context.Users.Add(baovinh);
            await context.SaveChangesAsync();

            var baovinhProfile = new ConsultantProfile
            {
                ConsultantId = baovinh.Id,
                Specialization = "Chuyển cấp & Thích nghi đại học",
                ExperienceYears = 1,
                HourlyRate = hourlyRate,
                Certifications = "Chưa có chứng chỉ chuyên môn",
                CreatedAt = now,
                UpdatedAt = now
            };
            context.ConsultantProfiles.Add(baovinhProfile);

            // 3️⃣ Consultant 3 - Cường Thịnh (male)
            var thinh = new User
            {
                FullName = "Cường Thịnh",
                Email = "thinhhpcse182037@fpt.edu.vn",
                PhoneNumber = "0123456788",
                Username = "thinh",
                Bio = "Sinh viên mới ra trường, tư vấn phát triển bản thân, tư duy tích cực và lộ trình sự nghiệp.",
                IsActive = true,
                UserRole = User.Role.Consultant,
                CreatedAt = now,
                UpdatedAt = now,
                Gender = true,
                DateOfBirth = new DateOnly(2001, 7, 4),
                MeetingLink = "https://meet.google.com/bvc-mjee-hwu",
                Avatar = "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=150&h=150&fit=crop",
            };
            thinh.Password = hasher.HashPassword(thinh, "123456");
            context.Users.Add(thinh);
            await context.SaveChangesAsync();

            var thinhProfile = new ConsultantProfile
            {
                ConsultantId = thinh.Id,
                Specialization = "Phát triển bản thân & Tư duy tích cực",
                ExperienceYears = 1,
                HourlyRate = hourlyRate,
                Certifications = "Chưa có chứng chỉ chuyên môn",
                CreatedAt = now,
                UpdatedAt = now
            };
            context.ConsultantProfiles.Add(thinhProfile);

            // 4️⃣ Consultant 4 - Hồng Như (female)
            var nhuvan = new User
            {
                FullName = "Hồng Như",
                Email = "nhuvhss170501@fpt.edu.vn",
                PhoneNumber = "0987654320",
                Username = "nhuvan",
                Bio = "Sinh viên mới ra trường, tư vấn định hướng nghề nghiệp trước khi ra trường và chứng chỉ nâng cao.",
                IsActive = true,
                UserRole = User.Role.Consultant,
                CreatedAt = now,
                UpdatedAt = now,
                Gender = false,
                DateOfBirth = new DateOnly(2000, 11, 15),
                MeetingLink = "https://meet.google.com/kpz-gikn-wvb",
                Avatar = "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=150&h=150&fit=crop",
            };
            nhuvan.Password = hasher.HashPassword(nhuvan, "123456");
            context.Users.Add(nhuvan);
            await context.SaveChangesAsync();

            var nhuvanProfile = new ConsultantProfile
            {
                ConsultantId = nhuvan.Id,
                Specialization = "Định hướng nghề nghiệp & Chứng chỉ nâng cao",
                ExperienceYears = 1,
                HourlyRate = hourlyRate,
                Certifications = "Chưa có chứng chỉ chuyên môn",
                CreatedAt = now,
                UpdatedAt = now
            };
            context.ConsultantProfiles.Add(nhuvanProfile);

            await context.SaveChangesAsync();

            // 5️⃣ Gắn lĩnh vực tư vấn (ConsultantConsulting)
            var duyenConsultings = context.Consultings.Where(c => new[] { "1", "4" }.Contains(c.Id.ToString()))
                .Select(c => new ConsultantConsulting { ConsultantId = duyen.Id, ConsultingId = c.Id }).ToList();

            var baovinhConsultings = context.Consultings.Where(c => new[] { "2", "3" }.Contains(c.Id.ToString()))
                .Select(c => new ConsultantConsulting { ConsultantId = baovinh.Id, ConsultingId = c.Id }).ToList();

            var thinhConsultings = context.Consultings.Where(c => new[] { "4", "5" }.Contains(c.Id.ToString()))
                .Select(c => new ConsultantConsulting { ConsultantId = thinh.Id, ConsultingId = c.Id }).ToList();

            var nhuvanConsultings = context.Consultings.Where(c => new[] { "5", "6" }.Contains(c.Id.ToString()))
                .Select(c => new ConsultantConsulting { ConsultantId = nhuvan.Id, ConsultingId = c.Id }).ToList();

            context.ConsultantConsultings.AddRange(duyenConsultings);
            context.ConsultantConsultings.AddRange(baovinhConsultings);
            context.ConsultantConsultings.AddRange(thinhConsultings);
            context.ConsultantConsultings.AddRange(nhuvanConsultings);

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

        private static async Task SeedAppointmentsAsync(RepositoryContext context)
        {
            if (context.Appointments.Any()) return;

            var now = DateTimeOffset.UtcNow;

            // Lấy danh sách client, consultant và schedule đã có
            var clients = context.Users.Where(u => u.UserRole == User.Role.Client).ToList();
            var consultants = context.Users.Where(u => u.UserRole == User.Role.Consultant).ToList();
            var schedules = context.Schedules.Include(s => s.Consultant).ToList();

            var appointments = new List<Appointment>();

            // 1️⃣ Appointment đang chờ xác nhận
            appointments.Add(new Appointment
            {
                ScheduleId = schedules[0].Id,
                ClientId = clients[0].Id,
                ConsultantId = schedules[0].ConsultantId,
                Status = AppointmentStatus.Pending,
                Amount = schedules[0].Price,
                Notes = "Tôi muốn tư vấn về định hướng học tập.",
                CreatedAt = now.AddDays(-1),
                UpdatedAt = now.AddDays(-1)
            });

            // 2️⃣ Appointment đã được xác nhận (chờ thanh toán)
            appointments.Add(new Appointment
            {
                ScheduleId = schedules[1].Id,
                ClientId = clients[1].Id,
                ConsultantId = schedules[1].ConsultantId,
                Status = AppointmentStatus.PendingPayment,
                PaymentStatus = PaymentStatus.PendingPayment,
                Amount = schedules[1].Price,
                Notes = "Tôi muốn được tư vấn về thích nghi đại học.",
                PaymentUrl = "https://payment.example.com/abc123",
                PaymentDueDate = schedules[1].StartTime.AddHours(-8),
                CreatedAt = now.AddDays(-2),
                UpdatedAt = now.AddDays(-2)
            });
            schedules[1].IsAvailable = false;

            // 3️⃣ Appointment đã hoàn thành
            appointments.Add(new Appointment
            {
                ScheduleId = schedules[2].Id,
                ClientId = clients[2].Id,
                ConsultantId = schedules[2].ConsultantId,
                Status = AppointmentStatus.Completed,
                Amount = schedules[2].Price,
                Notes = "Buổi tư vấn rất hữu ích, cảm ơn bạn!",
                CreatedAt = now.AddDays(-5),
                UpdatedAt = now.AddDays(-4)
            });
            schedules[2].IsAvailable = false;

            // 4️⃣ Appointment đã bị hủy
            appointments.Add(new Appointment
            {
                ScheduleId = schedules[3].Id,
                ClientId = clients[3].Id,
                ConsultantId = schedules[3].ConsultantId,
                Status = AppointmentStatus.Cancelled,
                Amount = schedules[3].Price,
                Notes = "Khách hàng bận việc đột xuất.",
                ReasonForCancellation = "Không thể tham dự đúng giờ",
                CreatedAt = now.AddDays(-3),
                UpdatedAt = now.AddDays(-3)
            });
            schedules[3].IsAvailable = true; // hủy nên có thể đặt lại

            await context.Appointments.AddRangeAsync(appointments);
            context.Schedules.UpdateRange(schedules);
            await context.SaveChangesAsync();
        }



    }
}
