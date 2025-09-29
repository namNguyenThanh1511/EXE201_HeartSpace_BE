using HeartSpace.Application.Services.AuthService.DTOs;
using HeartSpace.Application.Services.UserService.DTOs;
using HeartSpace.Domain.Entities;

namespace HeartSpace.Application.Extensions
{

    public static class UserMappingExtensions
    {
        public static User MapToUser(this UserCreationDto userCreationDto)
        {
            return new User
            {
                Username = userCreationDto.Username,
                Password = userCreationDto.Password,
                Email = userCreationDto.Email,
                PhoneNumber = userCreationDto.PhoneNumber,
                Identifier = userCreationDto.Identifier ?? null,
                Avatar = "https://media.istockphoto.com/vectors/default-profile-picture-avatar-photo-placeholder-vector-illustration-vector-id1223671392?k=6&m=1223671392&s=170667a&w=0&h=zP3l7WJinOFaGb2i1F4g8IS2ylw0FlIaa6x3tP9sebU=",
                IsActive = true,
                FullName = userCreationDto.FullName,
                DateOfBirth = userCreationDto.DateOfBirth ?? null,
            };
        }
        public static UserProfileResponse ToProfileResponse(this User user)
        {
            int? age = null;
            if (user.DateOfBirth != null)
            {
                age = CalculateAge(user.DateOfBirth.Value);
            }

            return new UserProfileResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Username = user.Username,
                DateOfBirth = user.DateOfBirth.ToString() ?? null,
                Identifier = user.Identifier ?? null,
                Avatar = user.Avatar,
                Role = user.UserRole.ToString(),
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Age = age,
                Gender = user.Gender.Value,
                IsAdult = age >= 18,


                //AvatarUrl = BuildAvatarUrl(user.Avatar, baseAvatarUrl)
            };
        }



        private static int? CalculateAge(DateOnly birthDate)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - birthDate.Year;

            if (birthDate > today.AddYears(-age))
                age--;

            return age;
        }

        //private static string BuildAvatarUrl(string? avatar, string? baseUrl)
        //{
        //    if (string.IsNullOrWhiteSpace(avatar))
        //        return string.Empty;

        //    if (avatar.StartsWith("http"))
        //        return avatar;

        //    return string.IsNullOrWhiteSpace(baseUrl)
        //        ? avatar
        //        : $"{baseUrl.TrimEnd('/')}/{avatar.TrimStart('/')}";
        //}

    }
}
