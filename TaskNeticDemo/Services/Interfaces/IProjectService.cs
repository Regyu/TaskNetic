using TaskNetic.Client.DTO;

namespace TaskNetic.Client.Services.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectInfo> GetProjectInfoAsync(int projectId);
    }
}
