namespace HeartSpace.Application.Services.AuthService.DTOs
{
    public class RegisterConsultantRequest : UserCreationDto
    {

        public string Specialization { get; set; } = string.Empty;
        public int ExperienceYears { get; set; }
        public decimal? HourlyRate { get; set; } = null;
        public string? Certifications { get; set; }

        // ==== Danh sách lĩnh vực tư vấn (Consulting) ====
        public List<int> ConsultingIds { get; set; } = new List<int>();

    }
}
