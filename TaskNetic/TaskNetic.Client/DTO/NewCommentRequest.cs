namespace TaskNetic.Client.DTO
{
    public class NewCommentRequest
    {
        public required string Comment {get; set; }
        public required string userId { get; set; }
        public DateTime creationDate { get; set; }
    }
}
