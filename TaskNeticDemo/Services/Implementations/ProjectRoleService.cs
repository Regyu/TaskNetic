using TaskNeticDemo.Services.Interfaces;
using TaskNeticDemo.Models;

namespace TaskNeticDemo.Services.Implementations
{
    public class ProjectRoleService : IProjectRoleService
    {
        private readonly ApplicationUserService _applicationUserService;
        public ProjectRoleService()
        {

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
            var user = await _applicationUserService.GetCurrentUserAsync();

            if (user == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var projectRole = await _context.ProjectRoles
                .FirstOrDefaultAsync(pr => pr.Project.Id == projectId && pr.ApplicationUser.Id == user.Id);

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
