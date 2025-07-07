
using TaskNeticDemo.Models;

namespace TaskNeticDemo.Services.Interfaces
{
    public interface IProjectService
    {
        Task<Project> GetProjectByIdAsync(int projectId);
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<Project> CreateProjectAsync(string projectName, string backgroundImageId);
        Task UpdateProjectAsync(Project project);
        Task DeleteProjectAsync(int projectId);
        Task<IEnumerable<ApplicationUser>> GetProjectMembersAsync(int projectId);
        Task AddUserToProjectAsync(int projectId, string userId, bool isAdmin);
        Task RemoveUserFromProjectAsync(int projectId, string userId);
    }
}
