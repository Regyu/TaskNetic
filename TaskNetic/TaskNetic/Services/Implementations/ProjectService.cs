using TaskNetic.Data;
using TaskNetic.Models;
using TaskNetic.Data.Repository;
using TaskNetic.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace TaskNetic.Services.Implementations
{
    public class ProjectService : Repository<Project>, IProjectService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ApplicationUserService _applicationUserService;

        public ProjectService(ApplicationDbContext context, AuthenticationStateProvider authenticationStateProvider)
            : base(context)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _applicationUserService = new ApplicationUserService(context, authenticationStateProvider);
        }

        public async Task<IEnumerable<Project>> GetCurrentUserProjectsAsync()
        {
            var user = await _applicationUserService.GetCurrentUserAsync();

            if (user == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            return await _context.ProjectRoles
                .Where(pr => pr.ApplicationUser.Id == user.Id)
                .Select(pr => pr.Project)
                .ToListAsync();
        }

        public async Task AddProjectWithCurrentUserAsync(Project project)
        {
            //var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            //var user = authState.User;
            var user = await _applicationUserService.GetCurrentUserAsync();

            if (user == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var currentUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (currentUser == null)
            {
                throw new InvalidOperationException("Current user not found.");
            }

            var projectRole = new ProjectRole
            {
                ApplicationUser = currentUser,
                Project = project,
                isAdmin = true
            };

            await _context.Projects.AddAsync(project);
            await _context.ProjectRoles.AddAsync(projectRole);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProjectAndUsersAsync(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project), "Project cannot be null.");
            }

            _context.ProjectRoles.RemoveRange(project.ProjectRoles);
            _context.Projects.Remove(project);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProjectRole>> GetProjectRoles(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project), "Project cannot be null.");
            }

            return await _context.ProjectRoles
        .Where(pr => pr.Project.Id == project.Id)
        .Include(pr => pr.ApplicationUser)
        .ToListAsync();
        }

        public async Task<string?> GetProjectBackgroundId(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project), "Project cannot be null.");
            }

            return project.BackgroundImageId;
        }

    }

}
