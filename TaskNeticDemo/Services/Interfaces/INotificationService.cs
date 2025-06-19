using TaskNeticDemo.Models;

namespace TaskNeticDemo.Services.Interfaces
{
    public interface INotificationService
    {
        Task<List<Notification>> GetCurrentUserNotificationsAsync();
    }
}
