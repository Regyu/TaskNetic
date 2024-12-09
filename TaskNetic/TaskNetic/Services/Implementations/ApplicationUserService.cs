using TaskNetic.Data;
using TaskNetic.Models;
using TaskNetic.Data.Repository;
using TaskNetic.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace TaskNetic.Services.Implementations
{
    public class ApplicationUserService : Repository<ApplicationUser>, IApplicationUserService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public ApplicationUserService(ApplicationDbContext context, AuthenticationStateProvider authenticationStateProvider) : base(context)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<ApplicationUser> GetByUserNameAsync(string userName)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == userName);
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }


        public async Task<ApplicationUser?> GetCurrentUserAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId != null)
                {
                    return await _context.Users.FindAsync(userId);
                }
            }

            return null;
        }

        public async Task<string?> GetCurrentUserIdAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            return user.Identity?.IsAuthenticated == true
                ? user.FindFirstValue(ClaimTypes.NameIdentifier)
                : null;
        }

        public async Task<bool> CheckIfUserExists(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
            }

            return await _context.Users.AnyAsync(u => u.Id == userId);
        }
    }
}
