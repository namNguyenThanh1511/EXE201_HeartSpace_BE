using HeartSpace.Domain.RequestFeatures;
using static HeartSpace.Domain.Entities.User;

namespace HeartSpace.Application.Services.UserService.DTOs
{
    public class UserQueryParams : RequestParameters
    {
        public string? SearchTerm { get; set; }
        public bool? Gender { get; set; }
        public Role Role { get; set; }
    }
}
