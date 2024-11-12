using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Interfaces
{
    public interface INotificationUserService : IRepository<NotificationUser>
    {
        Task<IEnumerable<NotificationUser>> GetNotificationsByUserIdAsync(string userId);
    }
}
