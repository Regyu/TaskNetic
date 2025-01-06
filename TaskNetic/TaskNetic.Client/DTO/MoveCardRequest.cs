namespace TaskNetic.Client.DTO
{
    public class MoveCardRequest
    {
        public int SourceListId { get; set; }
        public int TargetListId { get; set; }
        public int NewPosition { get; set; }
        public string CurrentUserId { get; set; }
    }
}
