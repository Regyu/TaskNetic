using Microsoft.EntityFrameworkCore;
using TaskNetic.Data;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Services.Implementations
{
    public class ProjectRoleService : Repository<ProjectRole>, IProjectRoleService
    {
        public ProjectRoleService(ApplicationDbContext context) : base(context) { }

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
    }
}
