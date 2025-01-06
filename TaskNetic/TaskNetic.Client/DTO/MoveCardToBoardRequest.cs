namespace TaskNetic.Client.DTO
{
    public class MoveCardToBoardRequest
    {
        public int CardId { get; set; }
        public int sourceListId { get; set; }
        public int targetListId { get; set; }

    }
}
