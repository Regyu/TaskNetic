using TaskNetic.Data;
using TaskNetic.Models;
using TaskNetic.Data.Repository;
using TaskNetic.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

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

        public async Task<string?> GetCurrentUserIdAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            return user.Identity?.IsAuthenticated == true
                ? user.FindFirstValue(ClaimTypes.NameIdentifier)
                : null;
        }
    }
}
