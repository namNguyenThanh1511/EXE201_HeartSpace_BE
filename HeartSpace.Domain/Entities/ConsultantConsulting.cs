namespace HeartSpace.Domain.Entities
{
    public class ConsultantConsulting
    {
        public int Id { get; set; }

        public int ConsultingId { get; set; }
        public Guid ConsultantId { get; set; }

        // Navigation properties
        public virtual Consulting Consulting { get; set; } = null!;
        public virtual User Consultant { get; set; } = null!;

    }
}
