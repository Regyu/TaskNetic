using TaskNetic.Data;
using TaskNetic.Models;
using TaskNetic.Data.Repository;
using TaskNetic.Services.Implementations;

namespace TaskNetic.Services.Interfaces
{
    public class ProjectService : Repository<Project>, IProjectService
    {
        public ProjectService(ApplicationDbContext context) : base(context) { }

        public Task<IEnumerable<Project>> GetUserProjectsAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
