using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetCurrentUserNotificationsAsync();
        Task AddNotificationAsync(string userId, string mentionedUserName, string message);
        Task AddNotificationAsync(ApplicationUser applicationUser, string mentionedUserName, string message);
        Task DeleteNotificationAsync(Notification notification);
    }
}
