using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Implementations
{
    public interface INotificationUserService : IRepository<NotificationUser>
    {
        Task<IEnumerable<NotificationUser>> GetNotificationsByUserIdAsync(string userId);
    }
}
