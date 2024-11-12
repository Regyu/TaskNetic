namespace TaskNetic.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int AuthorUserId { get; set; }
        public required string CommentText { get; set; }
        public DateTime timestamp { get; set; }
    }
}
