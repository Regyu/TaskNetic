namespace TaskNetic.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public required ApplicationUser User { get; set; }
        public required string Message { get; set; }
        public DateTime Time { get; set; }
    }
}
