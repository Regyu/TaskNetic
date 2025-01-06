namespace TaskNetic.Client.DTO
{
    public class NewBoardMember
    {
        public int boardId { get; set; }
        public string userName { get; set; }
        public bool canEdit { get; set; }
        public int projectId { get; set; }
        public string currentUserId { get; set; }
        public NewBoardMember(int boardId, string userName, bool canEdit, int projectId, string currentUserId)
        {
            this.boardId = boardId;
            this.userName = userName;
            this.canEdit = canEdit;
            this.projectId = projectId;
            this.currentUserId = currentUserId;
        }
    }
}
