using HeartSpace.Application.Exceptions;
using HeartSpace.Application.Extensions;
using HeartSpace.Application.Services.UserService.DTOs;
using HeartSpace.Domain.Entities;
using HeartSpace.Domain.Exception;
using HeartSpace.Domain.Repositories;

namespace HeartSpace.Application.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;


        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

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

        public async Task<UserProfileResponse?> GetUserProfileAsync(Guid userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);

            if (user == null)
                return null;

            // Check if user can access their profile (business rule)
            user.CheckCanLogin(); // This will throw if user is inactive

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

    }
}

