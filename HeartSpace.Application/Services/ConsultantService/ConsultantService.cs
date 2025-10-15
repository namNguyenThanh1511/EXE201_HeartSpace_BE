using HeartSpace.Application.Services.ConsultantService.DTOs;
using HeartSpace.Application.Services.UserService;
using HeartSpace.Application.Services.UserService.DTOs;
using HeartSpace.Domain.Entities;
using HeartSpace.Domain.Repositories;
using HeartSpace.Domain.RequestFeatures;

namespace HeartSpace.Application.Services.ConsultantService
{
    public class ConsultantService : IConsultantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        public ConsultantService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task<PagedList<ConsultantResponse>> GetConsultantsAsync(ConsultantQueryParams queryParams)
        {
            //(string userId, string role) = _currentUserService.GetCurrentUser();

            // 🔹 1. Chỉ cho phép các role nhất định xem danh sách Consultant
            //if (role != User.Role.Admin.ToString() && role != User.Role.Client.ToString())
            //    throw new InsufficientPermissionException("Bạn không có quyền xem danh sách chuyên gia.");

            // 🔹 2. Tạo query cơ sở: chỉ lấy user có role là Consultant và active
            IQueryable<User> query = _unitOfWork.Users.GetActiveConsultantsQueryable();

            // 🔹 3. Áp dụng các bộ lọc
            if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm))
            {
                string term = queryParams.SearchTerm.Trim().ToLower();
                query = query.Where(u =>
                    u.FullName.ToLower().Contains(term) ||
                    u.Email.ToLower().Contains(term) ||
                    u.Username.ToLower().Contains(term));
            }

            if (queryParams.Gender.HasValue)
            {
                query = query.Where(u => u.Gender == queryParams.Gender.Value);
            }

            if (queryParams.ConsultingsAt != null && queryParams.ConsultingsAt.Any())
            {
                query = query.Where(u =>
                    u.ConsultantConsultings.Any(cc =>
                        queryParams.ConsultingsAt.Contains(cc.Consulting.Id)));
            }


            // 🔹 4. Thực hiện phân trang
            var pagedConsultants = await PagedList<User>.ToPagedList(
                query.OrderBy(u => u.FullName),
                queryParams.PageNumber,
                queryParams.PageSize
            );

            // 🔹 5. Map sang ConsultantResponse DTO
            var consultantResponses = pagedConsultants.Select(u => new ConsultantResponse
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Username = u.Username,
                DateOfBirth = u.DateOfBirth.ToString(),
                PhoneNumber = u.PhoneNumber,
                Bio = u.Bio,
                Avatar = u.Avatar,
                Role = u.UserRole.ToString(),
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt,
                Gender = u.Gender,
                ConsultantInfo = new ConsultantDetailResponse
                {
                    Specialization = u.ConsultantProfile?.Specialization ?? string.Empty,
                    ExperienceYears = u.ConsultantProfile?.ExperienceYears ?? 0,
                    HourlyRate = u.ConsultantProfile?.HourlyRate,
                    Certifications = u.ConsultantProfile?.Certifications,
                    ConsultingIn = u.ConsultantConsultings
                    .Where(cc => cc.Consulting != null)
                    .Select(cc => new ConsultingsResponse
                    {
                        Id = cc.Consulting.Id.ToString(),
                        Name = cc.Consulting.Name,
                        Description = cc.Consulting.Description
                    })
                    .ToList()
                }

            }).ToList();

            // 🔹 6. Trả kết quả ra (đảm bảo giữ nguyên metadata phân trang)
            return new PagedList<ConsultantResponse>(
                consultantResponses,
                pagedConsultants.Count,
                queryParams.PageNumber,
                queryParams.PageSize
            );
        }


    }
}
