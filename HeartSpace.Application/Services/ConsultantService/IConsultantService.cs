using HeartSpace.Application.Services.ConsultantService.DTOs;
using HeartSpace.Domain.RequestFeatures;

namespace HeartSpace.Application.Services.ConsultantService
{
    public interface IConsultantService
    {
        Task<PagedList<ConsultantResponse>> GetConsultantsAsync(ConsultantQueryParams queryParams);

        Task<ConsultantResponse> GetConsultantByIdAsync(Guid id);


    }
}
