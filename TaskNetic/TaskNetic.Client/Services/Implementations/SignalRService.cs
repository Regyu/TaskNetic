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
                .WithUrl(navigationManager.ToAbsoluteUri("/applicationhub"))
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

        public async Task JoinBoardGroup(string userId, int boardId)
        {
            string groupName = boardId.ToString();
            await _hubConnection.SendAsync("JoinGroup", groupName);
        }

        public async Task LeaveGroupByUserId(string userId,int boardId)
        {
            string groupName = boardId.ToString();
            Console.WriteLine($"{userId} : usunięto z boarda {groupName}");
            await _hubConnection.SendAsync("LeaveGroup",userId, groupName);
        }
    
        public async Task NotifyGroupAboutListUpdate(int boardId)
        {
            string groupName = boardId.ToString();
            await _hubConnection.SendAsync("NotifyGroupAboutAddedList",groupName);
        }
        public async Task NotifyGroupAboutCardUpdate(int boardId)
        {
            string groupName = boardId.ToString();
            await _hubConnection.SendAsync("NotifyGroupAboutAddedCard",groupName);
        }
        public async Task NotifyUserAboutBoardUpdate(string userId)
        {
            await _hubConnection.SendAsync("NotifyUserAboutBoardUpdate", userId);
        }
        public async Task NotifyBoardGroupAboutUpdate(int boardId)
        {
            string groupName = boardId.ToString();
            Console.WriteLine($"Wysłano powiadomienie");
            await _hubConnection.SendAsync("NotifyBoardGroupAboutUpdate", groupName);
        }

        public void OnNotificationReceived(Func<Task> handler)
        {
            Console.WriteLine("Notification Received");
            _hubConnection.On("ReceiveBoardNotification", handler);
        }

        public void OnListUpdate(Func<Task> handler)
        {
            Console.WriteLine("Added new List !");
            _hubConnection.On("AddNewList", handler);
        }

        public void OnCardUpdate(Func<Task> handler)
        {
            Console.WriteLine("Added new Card !");
            _hubConnection.On("AddNewCard", handler);
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
