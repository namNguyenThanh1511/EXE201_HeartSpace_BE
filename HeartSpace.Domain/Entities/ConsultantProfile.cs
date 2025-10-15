namespace HeartSpace.Domain.Entities
{
    public class ConsultantProfile
    {
        public Guid Id { get; set; }
        public string Specialization { get; set; } = string.Empty;
        public int ExperienceYears { get; set; }
        public decimal? HourlyRate { get; set; }
        public string? Certifications { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        //fk
        public Guid ConsultantId { get; set; }
        // Navigation property
        public virtual User Consultant { get; set; } = null!;
    }
}
