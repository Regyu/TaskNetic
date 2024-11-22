using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Interfaces
{
    public interface IProjectService : IRepository<Project>
    {
        Task<IEnumerable<Project>> GetCurrentUserProjectsAsync();
        Task AddProjectWithCurrentUserAsync(Project project);
        Task DeleteProjectAndUsersAsync(Project project);
        Task<IEnumerable<ApplicationUser>> GetProjectUsers(Project project);
    }
}
