using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskNetic.Data;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.Diagnostics;

namespace TaskNetic.Services.Implementations
{
    public class BoardService : Repository<Board>, IBoardService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ApplicationUserService _applicationUserService;

        public BoardService(ApplicationDbContext context, AuthenticationStateProvider authenticationStateProvider) : base(context)
        {
            _applicationUserService = new ApplicationUserService(context, authenticationStateProvider);
        }


        public async Task<IEnumerable<Board>> GetBoardsByProjectAndUserIdAsync(int projectId, string userId)
        {
            if (!await _applicationUserService.CheckIfUserExists(userId))
                throw new InvalidOperationException("There is no user with that id.");

            var hasProjectRole = await _context.ProjectRoles
                .AnyAsync(pr => pr.ApplicationUser.Id == userId && pr.Project.Id == projectId);

            if (!hasProjectRole)
                throw new InvalidOperationException("The user does not have a role in this project.");

            var projectRoles = await _context.ProjectRoles
                .Where(pr => pr.ApplicationUser.Id == userId && pr.Project.Id == projectId)
                .Include(pr => pr.BoardPermissions)
                    .ThenInclude(bp => bp.Board)
                .ToListAsync();

            var boards = projectRoles
                .SelectMany(pr => pr.BoardPermissions.Select(bp => bp.Board))
                .Distinct()
                .ToList();

            return boards;
        }

        public async Task AddBoardByProjectAndUserIdAsync(int projectId, string userId, string boardTitle)
        {
            if (userId == null)
                throw new InvalidOperationException("No signed-in user found.");

            var project = await _context.Projects
            .Include(p => p.ProjectRoles)
            .ThenInclude(p => p.ApplicationUser)
            .FirstOrDefaultAsync(p => p.Id == projectId);
            

            if (project == null)
                throw new KeyNotFoundException($"Project with ID {projectId} not found.");

            var userRole = project.ProjectRoles.FirstOrDefault(pr => pr.ApplicationUser.Id == userId);
            if (userRole == null)
            {
                throw new InvalidOperationException("User does not have a role in this project.");
            }

            var newBoard = new Board
            {
                Title = boardTitle,
            };

            var boardPermission = new BoardPermission
            {
                Role = userRole,
                Board = newBoard,
                CanEdit = true
            };

            await _context.Boards.AddAsync(newBoard);
            await _context.BoardPermissions.AddAsync(boardPermission);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteBoardAsync(Board board)
        {
            if (board == null)
            {
                throw new ArgumentNullException(nameof(board), "Board cannot be null.");
            }

            await _context.Entry(board).Collection(b => b.BoardPermissions).LoadAsync();

            board.BoardPermissions.Clear();

            _context.Boards.Remove(board);

            await _context.SaveChangesAsync();
        }

    }
}
