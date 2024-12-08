using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Interfaces
{
    public interface INotificationService : IRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetCurrentUserNotificationsAsync();
        Task DeleteNotificationAsync(Notification notification);
    }
}
