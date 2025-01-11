using Microsoft.AspNetCore.SignalR;

namespace TaskNetic.Hubs
{
    public class ApplicationHub : Hub
    {
        private static int connectionsCounter = 0;
        private static readonly Dictionary<string, string> UserConnections = new();



        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
        public async Task AddUserToGroup(string groupName, string targetUserId)
        {
            if (UserConnections.TryGetValue(targetUserId, out var connectionId))
            {
                await Groups.AddToGroupAsync(connectionId, groupName);
                await Clients.Client(connectionId).SendAsync("ReceiveBoardNotification");
            }
            else
            {
                Console.WriteLine("Cannot find user!");
            }
        }
        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);         
        }
        public async Task NotifyGroupAboutProjectUpdate()
        {
            await Clients.All.SendAsync("ProjectUpdate");
        }
        public async Task NotifyBoardGroupAboutUpdate(string groupName)
        {
            await Clients.OthersInGroup(groupName).SendAsync("ReceiveBoardNotification");
        
        }
        public async Task NotifyUserAboutBoardUpdate(string userId)
        {
            await Clients.User(userId).SendAsync("ReceiveBoardNotification");
        }
        public async Task NotifyGroupAboutListUpdate(string groupName)
        {
            await Clients.OthersInGroup(groupName).SendAsync("ListUpdate");
        }
        public async Task NotifyGroupAboutCardUpdate(string groupName)
        {
            await Clients.OthersInGroup(groupName).SendAsync("CardUpdate");
        }
        public override Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            if (userId != null)
            {
                UserConnections[userId] = Context.ConnectionId;
            }
            connectionsCounter += 1;
            Console.WriteLine($"Connection started succesfully, connection count: {connectionsCounter} !");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;
            if (userId != null)
            {
                UserConnections.Remove(userId);
            }
            connectionsCounter -= 1;
            Console.WriteLine($"Connection finished succesfully, connection count: {connectionsCounter}!");
            return base.OnDisconnectedAsync(exception);
        }
      

    }
}