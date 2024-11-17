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

            return await _context.Projects
                .Where(p => p.ProjectUsers.Any(u => u.Id == userId))
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

            project.ProjectUsers.Add(currentUser);

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
        }
        /*public async Task DeleteProjectAndUsersAsync(int projectId) // usunąć
        {
            var project = await _context.Projects
                .Include(p => p.ProjectUsers)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                throw new KeyNotFoundException("Project ID is invalid.");
            }

            project.ProjectUsers.Clear();
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }*/

        public async Task DeleteProjectAndUsersAsync(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project), "Project cannot be null.");
            }

            _context.Entry(project).Collection(p => p.ProjectUsers).Load();

            project.ProjectUsers.Clear();

            _context.Projects.Remove(project);

            await _context.SaveChangesAsync();
        }

    }

}
