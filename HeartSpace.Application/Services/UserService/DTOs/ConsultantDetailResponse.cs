namespace HeartSpace.Application.Services.UserService.DTOs
{
    public class ConsultantDetailResponse
    {
        public List<ConsultingsResponse> ConsultingIn { get; set; } = new List<ConsultingsResponse>();
        public string Specialization { get; set; } = string.Empty;
        public int ExperienceYears { get; set; }
        public decimal? HourlyRate { get; set; }
        public string? Certifications { get; set; }
    }
    public class ConsultingsResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

}
