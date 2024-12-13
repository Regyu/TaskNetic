namespace TaskNetic.Client.Services.Interfaces
{
    public interface ISignalRService
    {
        Task StartAsync();
        void OnNotificationReceived(Func<Task> handler);
        Task NotifyUserAboutBoardUpdate(string userId);
        Task RegisterUser(string userId);
        Task NotifyAboutChange();
        Task StopConnectionAsync();
    }
}
