using System.ComponentModel.DataAnnotations;

namespace HeartSpace.Application.Services.ScheduleService.DTOs
{
    public class ScheduleCreation
    {
        [Required(ErrorMessage = "Thời gian bắt đầu là bắt buộc")]

        public DateTimeOffset StartTime { get; set; }
        [Required(ErrorMessage = "Thời gian kết thúc là bắt buộc")]
        public DateTimeOffset EndTime { get; set; }
    }
}
