using TaskNeticDemo.Models;
using TaskNeticDemo.Services.Interfaces;

namespace TaskNeticDemo.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private List<Notification> _notifications = new()
        {
            new Notification{Id=1, MentionedUserName="Lukas", Message="has moved card 'Update login logic' to list 'Done'.", Time=new DateTime(2025,6,15)},
            new Notification{Id=2, MentionedUserName="William", Message="has removed card 'Create user stories' from list 'Backlog'.", Time=new DateTime(2025,6,17)},
        };

        public async Task<List<Notification>> GetCurrentUserNotificationsAsync()
        {
            return await Task.FromResult(_notifications);
        }

    }
}
