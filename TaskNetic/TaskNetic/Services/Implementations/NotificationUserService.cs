using TaskNetic.Data;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Services.Implementations
{
    public class NotificationUserService : Repository<NotificationUser>, INotificationUserService
    {
        public NotificationUserService(ApplicationDbContext context) : base(context) { }

        public Task<IEnumerable<NotificationUser>> GetNotificationsByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
