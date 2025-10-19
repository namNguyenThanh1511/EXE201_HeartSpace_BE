using HeartSpace.Application.Services.UserService.DTOs;
using HeartSpace.Domain.Repositories;

namespace HeartSpace.Application.Services.ConsultingService
{
    public class ConsultingService : IConsultingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ConsultingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<ConsultingsResponse>> GetConsultings()
        {
            var consultings = await _unitOfWork.Consultings.GetAllAsync();
            return consultings.Select(c => new ConsultingsResponse
            {
                Id = c.Id.ToString(),
                Name = c.Name,
                Description = c.Description,
            }).ToList();
        }
    }
}
