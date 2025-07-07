using TaskNeticDemo.Models;
using TaskNeticDemo.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskNeticDemo.Services.Implementations
{
    public class ProjectRoleService : IProjectRoleService
    {
        private readonly IApplicationUserService _applicationUserService;
        private readonly List<ProjectRole> _projectRoles;

        public ProjectRoleService(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
            _projectRoles = new List<ProjectRole>();
            InitializeProjectRoles();
        }

        private void InitializeProjectRoles()
        {
            var users = _applicationUserService.GetAllUsers();

            var projects = new List<Project>
            {
                new Project { Id = 1, Name = "Project Alpha" },
                new Project { Id = 2, Name = "Project Beta" }
            };

            _projectRoles.Add(new ProjectRole { Id = 1, ApplicationUser = users[0], Project = projects[0], isAdmin = true });
            _projectRoles.Add(new ProjectRole { Id = 2, ApplicationUser = users[1], Project = projects[0], isAdmin = false });
            _projectRoles.Add(new ProjectRole { Id = 3, ApplicationUser = users[2], Project = projects[1], isAdmin = true });
            _projectRoles.Add(new ProjectRole { Id = 4, ApplicationUser = users[3], Project = projects[1], isAdmin = false });
            _projectRoles.Add(new ProjectRole { Id = 5, ApplicationUser = users[4], Project = projects[1], isAdmin = false });
        }

        public Task<IEnumerable<ProjectRole>> GetProjectRoleByUserId(string userId)
        {
            var roles = _projectRoles.Where(pr => pr.ApplicationUser.Id == userId);
            return Task.FromResult(roles.AsEnumerable());
        }

        public Task<IEnumerable<ProjectRole>> GetAllProjectRoles()
        {
            return Task.FromResult(_projectRoles.AsEnumerable());
        }

        public Task UpdateProjectRoleWithParametersAsync(int projectId, string userId, bool isAdmin)
        {
            var projectRole = _projectRoles.FirstOrDefault(pr => pr.Project.Id == projectId && pr.ApplicationUser.Id == userId);
            if (projectRole != null)
            {
                projectRole.isAdmin = isAdmin;
            }
            return Task.CompletedTask;
        }

        public Task RemoveUserFromProjectAsync(int projectId, string userId)
        {
            var projectRole = _projectRoles.FirstOrDefault(pr => pr.Project.Id == projectId && pr.ApplicationUser.Id == userId);
            if (projectRole != null)
            {
                _projectRoles.Remove(projectRole);
            }
            return Task.CompletedTask;
        }

        public async Task<bool> IsCurrentUserAdmin(int projectId)
        {
            var user = _applicationUserService.GetCurrentUserAsync();
            if (user == null)
            {
                return false;
            }
            var projectRole = _projectRoles.FirstOrDefault(pr => pr.Project.Id == projectId && pr.ApplicationUser.Id == user.Id);
            return projectRole?.isAdmin ?? false;
        }

        public Task<bool> IsUserAdminInProjectAsync(int projectId, string userId)
        {
            var projectRole = _projectRoles.FirstOrDefault(pr => pr.Project.Id == projectId && pr.ApplicationUser.Id == userId);
            return Task.FromResult(projectRole?.isAdmin ?? false);
        }
    }
}
