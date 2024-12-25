namespace TaskNetic.Client.DTO
{
    public class RemoveBoardMember
    {
        public int boardId { get; set; }
        public string userId { get; set; }
        public string currentUserId { get; set; }
        public RemoveBoardMember(int boardId, string userId, string currentUserId)
        {
            this.boardId = boardId;
            this.userId = userId;
            this.currentUserId = currentUserId;
        }
    }
}
