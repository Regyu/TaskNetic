using TaskNetic.Data;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Implementations;

namespace TaskNetic.Services.Interfaces
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
