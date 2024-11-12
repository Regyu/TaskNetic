using TaskNetic.Data;
using TaskNetic.Models;
using TaskNetic.Data.Repository;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Services.Implementations
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
