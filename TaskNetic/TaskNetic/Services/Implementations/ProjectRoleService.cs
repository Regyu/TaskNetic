using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskNetic.Data;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Services.Implementations
{
    public class ProjectRoleService : Repository<ProjectRole>, IProjectRoleService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public ProjectRoleService(ApplicationDbContext context, AuthenticationStateProvider authenticationStateProvider) : base(context)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        public Task<IEnumerable<ProjectRole>> GetProjectRoleByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateProjectRoleWithParametersAsync(int projectId, string userId, bool isAdmin)
        {
            var projectRole = await _context.ProjectRoles
                .FirstOrDefaultAsync(pr => pr.Project.Id == projectId && pr.ApplicationUser.Id == userId);

            if (projectRole == null)
            {
                throw new InvalidOperationException("Project role not found.");
            }

            projectRole.isAdmin = isAdmin;

            await _context.SaveChangesAsync();
        }

        public async Task RemoveUserFromProjectAsync(int projectId, string userId)
        {
            var projectRole = await _context.ProjectRoles
                .Include(pr => pr.BoardPermissions)
                .FirstOrDefaultAsync(pr => pr.Project.Id == projectId && pr.ApplicationUser.Id == userId);

            if (projectRole != null)
            {

                if (projectRole.BoardPermissions != null && projectRole.BoardPermissions.Any())
                {
                    _context.BoardPermissions.RemoveRange(projectRole.BoardPermissions);
                }

                _context.ProjectRoles.Remove(projectRole);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsCurrentUserAdmin(int projectId)
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

            var projectRole = await _context.ProjectRoles
                .FirstOrDefaultAsync(pr => pr.Project.Id == projectId && pr.ApplicationUser.Id == userId);


            return projectRole.isAdmin;
        }

        public async Task<bool> IsUserAdminInProjectAsync(int projectId, string userId)
        {
            var projectRole = await _context.ProjectRoles
                .FirstOrDefaultAsync(pr => pr.Project.Id == projectId && pr.ApplicationUser.Id == userId);

            return projectRole.isAdmin;
        }
    }
}
