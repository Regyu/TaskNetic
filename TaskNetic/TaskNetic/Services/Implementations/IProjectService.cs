using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Implementations
{
    public interface IProjectService : IRepository<Project>
    {
        Task<IEnumerable<Project>> GetUserProjectsAsync(string userId);
    }
}
