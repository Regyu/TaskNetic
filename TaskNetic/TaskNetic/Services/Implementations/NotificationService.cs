using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskNetic.Data;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Services.Implementations
{
    public class NotificationService : Repository<Notification>, INotificationService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public NotificationService(ApplicationDbContext context, AuthenticationStateProvider authenticationStateProvider) : base(context)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<IEnumerable<Notification>> GetCurrentUserNotificationsAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated != true)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new InvalidOperationException("User ID is not available.");
            }

            return await _context.Notifications
                .Where(nt => nt.User.Id == userId)
                .ToListAsync();
        }

        public async Task AddNotificationAsync(Notification notification, ApplicationUser applicationUser)
        {

            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification), "Notification cannot be null.");
            }

            if (applicationUser == null)
            {
                throw new ArgumentNullException(nameof(applicationUser), "User cannot be null.");
            }

            _context.Notifications.Add(notification);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteNotificationAsync(Notification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification), "Notification cannot be null.");
            }

            _context.Notifications.Remove(notification);

            await _context.SaveChangesAsync();
        }
    }
}