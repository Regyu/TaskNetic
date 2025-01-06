namespace TaskNetic.Client.DTO
{
    public class DueDateRequest
    {
        public DateTime? Date { get; set; }
        public required string UserId { get; set; }
    }
}
