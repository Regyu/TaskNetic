using Microsoft.AspNetCore.SignalR;

namespace TaskNetic.Hubs
{
    public class ApplicationHub : Hub
    {
        private static readonly Dictionary<string, string> _connections = new Dictionary<string, string>();

        public override Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier; 
            if (!_connections.ContainsKey(userId))
            {
                _connections.Add(userId, Context.ConnectionId);

            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;
            if (_connections.ContainsKey(userId))
            {
                _connections.Remove(userId);
            }

            return base.OnDisconnectedAsync(exception);
        }

   
        public async Task NotifyUserAboutBoardUpdate(string userId)
        {
            await Clients.User(userId).SendAsync("ReceiveBoardNotification");
        }

        public async Task NotifyAboutChange()
        {
            await Clients.Others.SendAsync("ReceiveBoardNotification");
        }
    }
}
