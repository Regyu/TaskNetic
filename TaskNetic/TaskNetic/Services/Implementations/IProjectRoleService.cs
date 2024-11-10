using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Implementations
{
    public interface IProjectRoleService : IRepository<ProjectRole>
    {
        Task<IEnumerable<ProjectRole>> GetProjectRoleById(string userId);
    }
}
