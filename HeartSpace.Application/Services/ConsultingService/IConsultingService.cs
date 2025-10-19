using HeartSpace.Application.Services.UserService.DTOs;

namespace HeartSpace.Application.Services.ConsultingService
{
    public interface IConsultingService
    {
        Task<List<ConsultingsResponse>> GetConsultings();
    }
}
