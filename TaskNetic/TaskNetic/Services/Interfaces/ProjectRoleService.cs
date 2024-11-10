using TaskNetic.Data;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Implementations;

namespace TaskNetic.Services.Interfaces
{
    public class ProjectRoleService : Repository<ProjectRole>, IProjectRoleService
    {
        public ProjectRoleService(ApplicationDbContext context) : base(context) { }

        public Task<IEnumerable<ProjectRole>> GetProjectRoleById(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
