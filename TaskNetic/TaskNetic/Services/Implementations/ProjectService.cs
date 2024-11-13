using TaskNetic.Data;
using TaskNetic.Models;
using TaskNetic.Data.Repository;
using TaskNetic.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace TaskNetic.Services.Implementations
{
    public class ProjectService : Repository<Project>, IProjectService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public ProjectService(ApplicationDbContext context, AuthenticationStateProvider authenticationStateProvider)
            : base(context)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<IEnumerable<Project>> GetCurrentUserProjectsAsync()
        {
            // Get the current user's ID from the AuthenticationStateProvider
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            // Ensure user is authenticated
            if (user.Identity?.IsAuthenticated != true)
            {
                return Enumerable.Empty<Project>();
            }

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Enumerable.Empty<Project>();
            }

            // Retrieve projects where the user is part of ProjectUsers
            return await _context.Projects
                .Where(p => p.ProjectUsers.Any(u => u.Id == userId))
                .ToListAsync();
        }
    }
}
