namespace HeartSpace.Domain.Entities
{
    public class Consulting
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTimeOffset CreateAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        // Navigation property: một Consulting có nhiều ConsultantConsulting
        public virtual ICollection<ConsultantConsulting> ConsultantConsultings { get; set; } = new List<ConsultantConsulting>();


    }
}
