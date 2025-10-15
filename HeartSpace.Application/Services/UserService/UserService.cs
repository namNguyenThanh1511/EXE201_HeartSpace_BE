using HeartSpace.Application.Exceptions;
using HeartSpace.Application.Extensions;
using HeartSpace.Application.Services.UserService.DTOs;
using HeartSpace.Domain.Entities;
using HeartSpace.Domain.Exception;
using HeartSpace.Domain.Repositories;
using HeartSpace.Domain.RequestFeatures;

namespace HeartSpace.Application.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserRepository _userRepository;


        public UserService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _userRepository = userRepository;
        }
        public async Task<User> FindUserByEmailAsync(string email)
        {
            // Use the repository to find the user by email
            var user = await _unitOfWork.Users.GetUserByEmailAsync(email) ?? throw new KeyNotFoundException($"User with email {email} not found");
            // Return the user if found
            if (!user.IsActive)
            {
                throw new KeyNotFoundException($"User with email {email} is not active");
            }
            return user;
        }


        public async Task<User> FindUserByIdAsync(string id)
        {
            return await _unitOfWork.Users.GetUserByIdentifierAsync(id);
        }

        public async Task<UserProfileResponse?> GetUserProfileAsync()
        {
            (string userId, string role) = _currentUserService.GetCurrentUser();

            User? user;
            if (role == User.Role.Consultant.ToString())
            {
                user = await _unitOfWork.Users.GetByIdWithProfileAsync(Guid.Parse(userId));
            }
            else
            {
                user = await _unitOfWork.Users.GetByIdAsync(Guid.Parse(userId));
            }

            if (user == null)
                throw new EntityNotFoundException("User not found");

            user.CheckCanLogin();

            return user.ToProfileResponse();
        }


        public async Task<User> FindUserByPhonenumberAsync(string phonenumber)
        {
            return await _unitOfWork.Users.GetUserByPhoneNumberAsync(phonenumber);
        }

        public async Task<User> FindUserByUsernameAsync(string username)
        {
            return await _unitOfWork.Users.GetUserByUserNameAsync(username);
        }
        public async Task SaveUserAsync(User user)
        {
            await _unitOfWork.Users.AddAsync(user);
        }
        public async Task UpdateUserAsync(User user)
        {
            await _unitOfWork.Users.UpdateAsync(user);
        }

        public async Task<bool> UpdateUserProfileAsync(Guid userId, UserProfileUpdateDto request)
        {
            var existingUser = await _unitOfWork.Users.GetByIdAsync(userId);
            if (existingUser == null)
            {
                throw new EntityNotFoundException("User not found", userId);
            }

            if (!existingUser.IsActive)
            {
                throw new UserInactiveException("Cannot update inactive user profile");
            }

            // Email
            if (!string.IsNullOrWhiteSpace(request.Email) &&
                !string.Equals(existingUser.Email, request.Email, StringComparison.OrdinalIgnoreCase))
            {
                var userEmailExists = await _unitOfWork.Users.GetUserByEmailAsync(request.Email);
                if (userEmailExists != null)
                {
                    throw new BusinessRuleViolationException($"Email {request.Email} is already in use");
                }

                existingUser.Email = request.Email.Trim().ToLowerInvariant();
            }

            // Phone
            if (!string.IsNullOrWhiteSpace(request.PhoneNumber) &&
                !string.Equals(existingUser.PhoneNumber, request.PhoneNumber, StringComparison.OrdinalIgnoreCase))
            {
                var userPhoneExists = await _unitOfWork.Users.GetUserByPhoneNumberAsync(request.PhoneNumber);
                if (userPhoneExists != null)
                {
                    throw new BusinessRuleViolationException($"Phone number {request.PhoneNumber} is already in use");
                }

                existingUser.PhoneNumber = request.PhoneNumber.Trim();
            }

            // FullName
            if (!string.IsNullOrWhiteSpace(request.FullName))
            {
                existingUser.FullName = request.FullName.Trim();
            }

            // DateOfBirth
            if (request.DateOfBirth.HasValue)
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                var age = today.Year - request.DateOfBirth.Value.Year;
                if (request.DateOfBirth > today.AddYears(-age)) age--;

                // if (age < 13 || age > 120)
                //     throw new BusinessRuleViolationException("Invalid age");

                existingUser.DateOfBirth = request.DateOfBirth.Value;
            }

            // Identifier
            if (!string.IsNullOrWhiteSpace(request.Identifier))
            {
                existingUser.Identifier = request.Identifier.Trim();
            }

            // Avatar
            if (!string.IsNullOrWhiteSpace(request.Avatar))
            {
                existingUser.Avatar = request.Avatar.Trim();
            }

            // Gender
            if (request.Gender.HasValue)
            {
                existingUser.Gender = request.Gender.Value;
            }

            existingUser.UpdatedAt = DateTimeOffset.UtcNow;

            try
            {
                _unitOfWork.Users.Update(existingUser);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception e)
            {
                throw new DatabaseException("Cannot update profile", e);
            }

            return true;
        }

        public async Task<PagedList<UserProfileResponse>> GetAllUserProfileAsync(UserQueryParams queryParams)
        {


            // 2. Tạo query cơ sở
            IQueryable<User> query = _unitOfWork.Users.FindByCondition((a => a.IsActive));


            if (queryParams.Role == User.Role.Consultant)
            {
                query = query.Where(u => u.UserRole == User.Role.Consultant);
            }
            else if (queryParams.Role == User.Role.Client)
            {
                query = query.Where(u => u.UserRole == User.Role.Client);
            }
            else if (queryParams.Role == User.Role.Admin)
            {
                query = query.Where(u => u.UserRole == User.Role.Admin);
            }
            if (queryParams.SearchTerm is not null)
            {
                var searchTerm = queryParams.SearchTerm.Trim().ToLower();
                query = query.Where(u => u.FullName.ToLower().Contains(searchTerm) ||
                                         u.Email.ToLower().Contains(searchTerm) ||
                                         (u.PhoneNumber != null && u.PhoneNumber.ToLower().Contains(searchTerm)) ||
                                         u.Username.ToLower().Contains(searchTerm));
            }
            // 3. Áp dụng bộ lọc từ queryParams nếu có
            if (queryParams.Gender.HasValue)
            {
                query = query.Where(u => u.Gender == queryParams.Gender.Value);
            }


            // 5. Include navigation properties nếu cần


            // 6. Thực hiện phân trang
            var pagedUser = await PagedList<User>.ToPagedList(
                query.OrderByDescending(a => a.CreatedAt),
                queryParams.PageNumber,
                queryParams.PageSize
            );

            // 7. Map sang DTO (AppointmentResponse)
            var mappedUsers = pagedUser.Select(a => a.ToProfileResponse()).ToList();

            var result = new PagedList<UserProfileResponse>(
                mappedUsers,
                pagedUser.MetaData.TotalCount,
                pagedUser.MetaData.CurrentPage,
                pagedUser.MetaData.PageSize
            );
            return result;
        }

    }
}

