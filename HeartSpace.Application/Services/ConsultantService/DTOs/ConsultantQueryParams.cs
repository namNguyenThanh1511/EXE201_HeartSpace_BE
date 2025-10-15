using HeartSpace.Domain.RequestFeatures;

namespace HeartSpace.Application.Services.ConsultantService.DTOs
{
    public class ConsultantQueryParams : RequestParameters
    {
        public string? SearchTerm { get; set; }
        public bool? Gender { get; set; }
        public List<int> ConsultingsAt { get; set; }
    }
}
