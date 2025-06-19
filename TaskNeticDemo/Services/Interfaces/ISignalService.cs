using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace TaskNetic.Client.Services.Interfaces
{
    public interface ISignalRService
    {
        Task StartAsync();
        void OnNotificationReceived(Func<Task> handler);
        void OnListUpdate(Func<Task> handler);
        void OnCardUpdate(Func<Task> handler);
        void OnPermissionUpdate(Func<bool, Task> handler);
        Task NotifyUserAboutBoardUpdate(string userId);
        Task NotifyUserAboutPermissionChange(string userId, bool canEdit);
        Task AddUserToGroup(string userId, int groupId);
        Task NotifyBoardGroupAboutUpdate(int boardId);
        Task NotifyGroupAboutListUpdate(int boardId);
        Task NotifyGroupAboutCardUpdate(int boardId);
        Task JoinBoardGroup(string userId, int boardId);
        Task LeaveGroupByUserId(string userId, int boardId);
        Task StopConnectionAsync();      

    }
}
