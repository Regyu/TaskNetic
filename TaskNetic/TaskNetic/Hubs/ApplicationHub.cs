using Microsoft.AspNetCore.SignalR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TaskNetic.Hubs
{
    public class ApplicationHub : Hub
    {
        private static readonly Dictionary<string, HashSet<string>> GroupConnections = new();

        
        public async Task JoinGroup(string groupName)
        {
            
            if (!GroupConnections.ContainsKey(groupName))
            {
                GroupConnections[groupName] = new HashSet<string>();
            }

            
            if (GroupConnections[groupName].Contains(Context.ConnectionId))
            {                
                return;
            }
            
            GroupConnections[groupName].Add(Context.ConnectionId);
            
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        
        public async Task LeaveGroup(string groupName)
        {
            
            if (GroupConnections.ContainsKey(groupName) && GroupConnections[groupName].Contains(Context.ConnectionId))
            {
                
                GroupConnections[groupName].Remove(Context.ConnectionId);

                
                if (GroupConnections[groupName].Count == 0)
                {
                    GroupConnections.Remove(groupName);
                }

                
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
         
            }
        }
        public async Task NotifyBoardGroupAboutUpdate(string groupName)
        {
            await Clients.Group(groupName).SendAsync("ReceiveBoardNotification");
        }

        public async Task NotifyUserAboutBoardUpdate(string userId)
        {         
         await Clients.User(userId).SendAsync("ReceiveBoardNotification");
        }
        public async Task NotifyGroupAboutAddedList(string groupName)
        {
            await Clients.Group(groupName).SendAsync("AddNewList");
        }
        public async Task NotifyGroupAboutAddedCard(string groupName)
        {
            await Clients.Group(groupName).SendAsync("AddNewCard");
        }
       

        public override Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;
            return base.OnDisconnectedAsync(exception);
        }

    }
}
