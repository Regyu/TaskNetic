using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskNetic.Data;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public NotificationService(IDbContextFactory<ApplicationDbContext> dbContextFactory, AuthenticationStateProvider authenticationStateProvider)
        {
            _dbContextFactory = dbContextFactory;
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

            using var context = _dbContextFactory.CreateDbContext();
            return await context.Notifications
                .Where(nt => nt.User.Id == userId)
                .ToListAsync();
        }

        public async Task AddNotificationAsync(string userId, string mentionedUserName, string message)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("UserId cannot be null or empty.", nameof(mentionedUserName));
            }

            if (string.IsNullOrWhiteSpace(mentionedUserName))
            {
                throw new ArgumentException("Mentioned user name cannot be null or empty.", nameof(mentionedUserName));
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Message cannot be null or empty.", nameof(message));
            }

            using var context = _dbContextFactory.CreateDbContext();

            var applicationUser = await context.Users.FindAsync(userId);

            if (applicationUser == null)
            {
                throw new ArgumentException("User not found.", nameof(userId));
            }

            var notification = new Notification
            {
                User = applicationUser,
                MentionedUserName = mentionedUserName,
                Message = message,
                Time = DateTime.UtcNow
            };

            context.Notifications.Add(notification);
            await context.SaveChangesAsync();
        }

        public async Task AddNotificationAsync(ApplicationUser applicationUser, string mentionedUserName, string message)
        {
            if (applicationUser == null)
            {
                throw new ArgumentNullException(nameof(applicationUser), "User cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(mentionedUserName))
            {
                throw new ArgumentException("Mentioned user name cannot be null or empty.", nameof(mentionedUserName));
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Message cannot be null or empty.", nameof(message));
            }

            var notification = new Notification
            {
                User = applicationUser,
                MentionedUserName = mentionedUserName,
                Message = message,
                Time = DateTime.UtcNow
            };

            using var context = _dbContextFactory.CreateDbContext();
            context.Notifications.Add(notification);
            await context.SaveChangesAsync();
        }
        public async Task DeleteNotificationAsync(Notification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification), "Notification cannot be null.");
            }

            using var context = _dbContextFactory.CreateDbContext();
            context.Notifications.Remove(notification);
            await context.SaveChangesAsync();
        }

    }
}
