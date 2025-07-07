using TaskNeticDemo.Models;
using TaskNeticDemo.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskNetic.Client.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRoleService _projectRoleService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly List<Project> _projects;

        public ProjectService(IProjectRoleService projectRoleService, IApplicationUserService applicationUserService)
        {
            _projectRoleService = projectRoleService;
            _applicationUserService = applicationUserService;
            _projects = new List<Project>
            {
                new Project { Id = 1, ProjectName = "TaskNetic Development", BackgroundImageId = "background1.jpg" },
                new Project { Id = 2, ProjectName = "Marketing Campaign", BackgroundImageId = "background2.jpg" },
                new Project { Id = 3, ProjectName = "Website Redesign", BackgroundImageId = "background3.jpg" }
            };
        }

        public Task<Project> GetProjectByIdAsync(int projectId)
        {
            var project = _projects.FirstOrDefault(p => p.Id == projectId);
            return Task.FromResult(project);
        }

        public Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return Task.FromResult(_projects.AsEnumerable());
        }

        public Task<Project> CreateProjectAsync(string projectName, string backgroundImageId)
        {
            var newProject = new Project
            {
                Id = _projects.Max(p => p.Id) + 1,
                ProjectName = projectName,
                BackgroundImageId = backgroundImageId
            };
            _projects.Add(newProject);
            return Task.FromResult(newProject);
        }

        public Task UpdateProjectAsync(Project project)
        {
            var projectToUpdate = _projects.FirstOrDefault(p => p.Id == project.Id);
            if (projectToUpdate != null)
            {
                projectToUpdate.ProjectName = project.ProjectName;
                projectToUpdate.BackgroundImageId = project.BackgroundImageId;
            }
            return Task.CompletedTask;
        }

        public Task DeleteProjectAsync(int projectId)
        {
            var projectToRemove = _projects.FirstOrDefault(p => p.Id == projectId);
            if (projectToRemove != null)
            {
                _projects.Remove(projectToRemove);
            }
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<ApplicationUser>> GetProjectMembersAsync(int projectId)
        {
            var allRoles = await _projectRoleService.GetAllProjectRoles();
            var userIds = allRoles.Where(pr => pr.Project.Id == projectId).Select(pr => pr.ApplicationUser.Id);
            var users = _applicationUserService.GetAllUsers().Where(u => userIds.Contains(u.Id));
            return users;
        }

        public async Task AddUserToProjectAsync(int projectId, string userId, bool isAdmin)
        {
            await _projectRoleService.UpdateProjectRoleWithParametersAsync(projectId, userId, isAdmin);
        }

        public async Task RemoveUserFromProjectAsync(int projectId, string userId)
        {
            await _projectRoleService.RemoveUserFromProjectAsync(projectId, userId);
        }
    }
}
