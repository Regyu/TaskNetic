using System.Data.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using TaskNetic.Client.Pages;
using TaskNetic.Client.Services.Interfaces;

namespace TaskNetic.Client.Services.Implementations
{
    public class SignalRService : ISignalRService, IAsyncDisposable
    {
        private readonly HubConnection _hubConnection;
        private bool _isConnected = false;

        public SignalRService(NavigationManager navigationManager)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(navigationManager.ToAbsoluteUri("/applicationhub"))
                .Build();
        }

        public async Task StartAsync()
        {
            if (_hubConnection.State == HubConnectionState.Disconnected)
            {
                await _hubConnection.StartAsync();

            }
        }

        public async Task JoinBoardGroup(string userId, int boardId)
        {
            string groupName = boardId.ToString();
            await _hubConnection.SendAsync("JoinGroup", groupName);
        }
        public async Task AddUserToGroup(string userId, int boardId)
        {
            string groupName = boardId.ToString();
            await _hubConnection.SendAsync("AddUserToGroup", groupName, userId);
        }
        public async Task LeaveGroupByUserId(string userId,int boardId)
        {
            string groupName = boardId.ToString();
            await _hubConnection.SendAsync("LeaveGroup",userId, groupName);
        }
    
        public async Task NotifyGroupAboutListUpdate(int boardId)
        {
            string groupName = boardId.ToString();
            await _hubConnection.SendAsync("NotifyGroupAboutListUpdate",groupName);
        }
        public async Task NotifyGroupAboutCardUpdate(int boardId)
        {
            string groupName = boardId.ToString();
            await _hubConnection.SendAsync("NotifyGroupAboutCardUpdate",groupName);
        }
        public async Task NotifyUserAboutBoardUpdate(string userId)
        {
            await _hubConnection.SendAsync("NotifyUserAboutBoardUpdate", userId);
        }
        public async Task NotifyBoardGroupAboutUpdate(int boardId)
        {
            string groupName = boardId.ToString();            
            await _hubConnection.SendAsync("NotifyBoardGroupAboutUpdate", groupName);
        }

        public void OnNotificationReceived(Func<Task> handler)
        {            
            _hubConnection.On("ReceiveBoardNotification", handler);
        }

        public void OnListUpdate(Func<Task> handler)
        {
            _hubConnection.On("ListUpdate", handler);
        }

        public void OnCardUpdate(Func<Task> handler)
        {
            _hubConnection.On("CardUpdate", handler);
        }
        public async Task StopConnectionAsync()
        {
            if (_hubConnection != null && _hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.StopAsync();
                await _hubConnection.DisposeAsync();
                _isConnected = false;
            }
        }
        public async ValueTask DisposeAsync()
        {
            await _hubConnection.StopAsync();
        }

    }
}
