using TaskNetic.Data;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Services.Implementations
{
    public class NotificationService : Repository<NotificationUser>, INotificationService
    {
        public NotificationService(ApplicationDbContext context) : base(context) { }

        public Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}