namespace TaskNetic.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public required ApplicationUser User { get; set; }
        public required string CommentText { get; set; }
        public DateTime timestamp { get; set; }
    }
}
