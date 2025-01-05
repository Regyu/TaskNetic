using Microsoft.AspNetCore.SignalR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TaskNetic.Hubs
{
    public class ApplicationHub : Hub
    {
        private static int number = 0;
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
        public async Task NotifyAboutProjectUpdate(string userId)
        {
            Console.WriteLine("Odebrano w Hubie !");
            await Clients.Client(userId).SendAsync("ProjectUpdate");
        }

        public override Task OnConnectedAsync()
        {
            number+=1;
            Console.WriteLine($"Connection started succesfully, connection count: {number} !");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            number-=1;
            Console.WriteLine($"Connection finished succesfully connection count: {number}!");
            return base.OnDisconnectedAsync(exception);
        }
      

    }
}