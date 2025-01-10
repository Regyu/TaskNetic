namespace TaskNetic.Client.DTO
{
    public class LabelCardRequest
    {
        public int LabelId { get; set; }
        public required string CurrentUserId { get; set; }
    }
}
