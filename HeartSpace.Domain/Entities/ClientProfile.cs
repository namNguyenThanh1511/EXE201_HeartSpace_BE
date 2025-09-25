namespace HeartSpace.Domain.Entities
{
    public class ClientProfile
    {
        public Guid Id { get; set; }
        public string Bio { get; set; } = string.Empty;
        public string Preferences { get; set; } = string.Empty;
        public string Goals { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string MedicalHistory { get; set; } = string.Empty;
        public string MentalHealthStatus { get; set; } = string.Empty;
        //fk
        public Guid ClientId { get; set; }
        // Navigation property
        public virtual User Client { get; set; } = null!;
    }
}
