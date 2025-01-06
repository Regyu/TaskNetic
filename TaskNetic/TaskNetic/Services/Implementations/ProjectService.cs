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

            _context.Entry(project).Collection(p => p.ProjectRoles).Load();
            _context.Entry(project).Collection(p => p.ProjectBoards).Load();

            foreach (var board in project.ProjectBoards)
            {
                _context.Entry(board).Collection(b => b.BoardPermissions).Load();
                _context.Entry(board).Collection(b => b.Lists).Load();
                _context.Entry(board).Collection(b => b.Labels).Load();

                foreach (var list in board.Lists)
                {
                    _context.Entry(list).Collection(l => l.Cards).Load();

                    foreach (var card in list.Cards)
                    {
                        _context.Entry(card).Collection(c => c.Comments).Load();
                        _context.Entry(card).Collection(c => c.Attachments).Load();
                        _context.Entry(card).Collection(c => c.TodoTasks).Load();
                        _context.Entry(card).Collection(c => c.CardLabels).Load();
                        _context.Entry(card).Collection(c => c.CardMembers).Load();

                        _context.Comments.RemoveRange(card.Comments);
                        _context.Attachments.RemoveRange(card.Attachments);
                        _context.TodoTasks.RemoveRange(card.TodoTasks);

                        card.CardLabels.Clear();
                        card.CardMembers.Clear();

                        _context.Cards.Remove(card);
                    }
                    _context.Cards.RemoveRange(list.Cards);
                    _context.Lists.Remove(list);
                }
                _context.BoardPermissions.RemoveRange(board.BoardPermissions);

                _context.RemoveRange(board.Lists);
                _context.RemoveRange(board.Labels);

                _context.Boards.Remove(board);
            }
            _context.Boards.RemoveRange(project.ProjectBoards);
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
