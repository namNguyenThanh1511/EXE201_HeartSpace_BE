namespace HeartSpace.Domain.Entities
{
    public class Meeting
    {
        public int Id { get; set; }
        public int User1Id { get; set; }  // Người tạo meeting
        public int User2Id { get; set; }  // Người tham gia
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public MeetingStatus Status { get; set; } = MeetingStatus.Pending;  // Pending, Active, Ended, Cancelled
        public string RoomId { get; set; }  // Google Meet conference ID
        public string MeetLink { get; set; }  // https://meet.google.com/abc-defg-hij
        public User User1 { get; set; }
        public User User2 { get; set; }

        public enum MeetingStatus
        {
            Pending,
            Active,
            Ended,
            Cancelled
        }
    }
}
