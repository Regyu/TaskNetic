namespace TaskNetic.Client.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public required NotificationUser User { get; set; }
        public required string Message { get; set; }
        public DateTime Time { get; set; }
    }
}
