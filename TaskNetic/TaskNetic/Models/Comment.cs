namespace TaskNetic.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int AuthorUserId { get; set; }
        public required string CommentText { get; set; }
        DateTime timestamp { get; set; }
    }
}
