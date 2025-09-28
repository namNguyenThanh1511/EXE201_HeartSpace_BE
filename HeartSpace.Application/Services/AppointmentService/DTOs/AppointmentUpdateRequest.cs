namespace HeartSpace.Application.Services.AppointmentService.DTOs
{
    public class AppointmentUpdateRequest
    {
        public UpdateFor For { get; set; } // "Confirm appointment" or "Cancel appointment"
        public string? Notes { get; set; }
        public string? ReasonForCancellation { get; set; }

        public Guid? NewScheduleId { get; set; } // For rescheduling

        public enum UpdateFor
        {
            ConfirmAppointment,
            CompleteAppointment,
            CancelAppointment,
            RescheduleAppointment,
            AddNotes
        }
    }
}
