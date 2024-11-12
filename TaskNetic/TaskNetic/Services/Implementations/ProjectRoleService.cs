using TaskNetic.Data;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Services.Implementations
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
