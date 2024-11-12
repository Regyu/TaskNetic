using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Interfaces
{
    public interface IProjectRoleService : IRepository<ProjectRole>
    {
        Task<IEnumerable<ProjectRole>> GetProjectRoleById(string userId);
    }
}
