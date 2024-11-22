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

        public ProjectService(ApplicationDbContext context, AuthenticationStateProvider authenticationStateProvider)
            : base(context)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<IEnumerable<Project>> GetCurrentUserProjectsAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated != true)
            {
                return Enumerable.Empty<Project>();
            }

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Enumerable.Empty<Project>();
            }

            return await _context.ProjectRoles
                .Where(pr => pr.ApplicationUser.Id == userId)
                .Select(pr => pr.Project)
                .ToListAsync();
        }

        public async Task AddProjectWithCurrentUserAsync(Project project)
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated != true)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new InvalidOperationException("User ID is not available.");
            }

            var currentUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

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

        public async Task<IEnumerable<ApplicationUser>> GetProjectUsers(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project), "Project cannot be null.");
            }

            return await _context.ProjectRoles
            .Where(pr => pr.Project.Id == project.Id)
            .Select(pr => pr.ApplicationUser)
            .Distinct()
            .ToListAsync();
        }
    }

}
