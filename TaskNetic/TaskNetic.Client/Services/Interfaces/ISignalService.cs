namespace TaskNetic.Client.Services.Interfaces
{
    public interface ISignalRService
    {
        Task StartAsync();
        void OnNotificationReceived(Func<Task> handler);
        void OnListUpdate(Func<Task> handler);
        void OnCardUpdate(Func<Task> handler);
        Task NotifyUserAboutBoardUpdate(string userId);
        Task NotifyBoardGroupAboutUpdate(int boardId);
        Task NotifyGroupAboutListUpdate(int boardId);
        Task NotifyGroupAboutCardUpdate(int boardId);
        Task JoinBoardGroup(string userId, int boardId);
        Task LeaveGroupByUserId(string userId, int boardId);
        Task RegisterUser(string userId);
        Task StopConnectionAsync();
    }
}
