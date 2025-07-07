using TaskNeticDemo.Models;

namespace TaskNeticDemo.Services.Interfaces
{
    public interface IProjectRoleService
    {
        Task<IEnumerable<ProjectRole>> GetProjectRoleByUserId(string userId);
        Task<IEnumerable<ProjectRole>> GetAllProjectRoles();
        Task UpdateProjectRoleWithParametersAsync(int projectId, string userId, bool isAdmin);
        Task RemoveUserFromProjectAsync(int projectId, string userId);
        Task<bool> IsCurrentUserAdmin(int projectId);
        Task<bool> IsUserAdminInProjectAsync(int projectId, string userId);
    }
}
