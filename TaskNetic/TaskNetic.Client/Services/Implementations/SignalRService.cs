using System.Data.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using TaskNetic.Client.Pages;
using TaskNetic.Client.Services.Interfaces;

namespace TaskNetic.Client.Services.Implementations
{
    public class SignalRService : ISignalRService
    {
        private readonly HubConnection _hubConnection;

        public SignalRService(NavigationManager navigationManager)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://tasknetic.service.signalr.net/applicationhub")
                .Build();
        }

        public async Task RegisterUser(string userId)
        {
            await _hubConnection.SendAsync("RegisterUser", userId);
        }

        public async Task StartAsync()
        {
            if (_hubConnection.State == HubConnectionState.Disconnected)
            {
                await _hubConnection.StartAsync();

            }
        }

        public async Task NotifyUserAboutBoardUpdate(string userId)
        {
            await _hubConnection.SendAsync("NotifyUserAboutBoardUpdate", userId);
        }


        public void OnNotificationReceived(Func<Task> handler)
        {           
            _hubConnection.On("ReceiveBoardNotification", handler);
        }
        public async Task NotifyAboutChange()
        {
            await _hubConnection.SendAsync("NotifyAboutChange");
        }

        public async Task StopConnectionAsync()
        {
            if (_hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.StopAsync();
            }
        }
    }
}
