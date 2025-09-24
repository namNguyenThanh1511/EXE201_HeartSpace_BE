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

            // Get existing user
            var existingUser = await _unitOfWork.Users.GetByIdAsync(userId);
            if (existingUser == null)
            {
                throw new EntityNotFoundException("User not found", userId);
            }

            if (!existingUser.IsActive)
            {
                throw new UserInactiveException("Cannot update inactive user profile");
            }

            // Check if email is being changed and if it's already in use
            if (!string.Equals(existingUser.Email, request.Email, StringComparison.OrdinalIgnoreCase))
            {
                var userEmailExists = await FindUserByEmailAsync(request.Email);
                if (userEmailExists != null)
                {
                    throw new BusinessRuleViolationException($"Email {request.Email} is already in use");
                }
            }

            // Validate age (example: minimum 13 years old)
            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - request.DateOfBirth.Year;
            if (request.DateOfBirth > today.AddYears(-age)) age--;

            //if (age < 13)
            //{
            //    throw new BusinessRuleViolationException("User must be at least 13 years old");
            //}

            //if (age > 120)
            //{
            //    throw new BusinessRuleViolationException("Invalid date of birth");
            //}
            // Update user properties
            existingUser.FullName = request.FullName.Trim();
            existingUser.Email = request.Email.Trim().ToLowerInvariant();
            existingUser.PhoneNumber = string.IsNullOrWhiteSpace(request.PhoneNumber)
                ? null
                : request.PhoneNumber.Trim();
            existingUser.DateOfBirth = request.DateOfBirth;
            existingUser.Identifier = string.IsNullOrWhiteSpace(request.Identifier)
                ? existingUser.Identifier
                : request.Identifier.Trim();
            existingUser.Avatar = string.IsNullOrWhiteSpace(request.Avatar)
                ? null
                : request.Avatar.Trim();
            existingUser.UpdatedAt = DateTimeOffset.UtcNow;

            // Update in database
            try
            {
                _unitOfWork.Users.Update(existingUser);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception e)
            {
                throw new DatabaseException("Cannot update profile of an inactive user", e);
            }

            return true;
        }
    }
}

