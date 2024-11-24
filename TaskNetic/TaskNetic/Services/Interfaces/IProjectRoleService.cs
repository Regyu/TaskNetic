using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Interfaces
{
    public interface IProjectRoleService : IRepository<ProjectRole>
    {
        Task<IEnumerable<ProjectRole>> GetProjectRoleByUserId(string userId);

        Task UpdateProjectRoleWithParametersAsync(int projectId, string userId, bool isAdmin);
        Task RemoveUserFromProjectAsync(int projectId, string userId);
    }
}
