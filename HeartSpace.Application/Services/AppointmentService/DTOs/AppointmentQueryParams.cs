using HeartSpace.Domain.Entities;
using HeartSpace.Domain.RequestFeatures;

namespace HeartSpace.Application.Services.AppointmentService.DTOs
{
    public class AppointmentQueryParams : RequestParameters
    {
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public Guid? ConsultantId { get; set; }
        public Guid? ClientId { get; set; }
        public AppointmentStatus? Status { get; set; }
    }
}
