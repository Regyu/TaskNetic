using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskNetic.Data;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;

namespace TaskNetic.Services.Implementations
{
    public class BoardService : Repository<Board>, IBoardService
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public BoardService(ApplicationDbContext context, AuthenticationStateProvider authenticationStateProvider) : base(context)
        {
            _context = context;
            _authenticationStateProvider = authenticationStateProvider;
        }

        private async Task<string?> GetCurrentUserIdAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            return user.Identity?.IsAuthenticated == true
                ? user.FindFirstValue(ClaimTypes.NameIdentifier)
                : null;
        }

        public async Task<IEnumerable<Board>> GetBoardsByProjectIdForCurrentUserAsync(int projectId)
        {
            var currentUserId = await GetCurrentUserIdAsync();

            if (currentUserId == null)
                throw new InvalidOperationException("No signed-in user found.");

            var projectRoles = await _context.ProjectRoles
                .Where(pr => pr.ApplicationUser.Id == currentUserId && pr.Project.Id == projectId)
                .Include(pr => pr.BoardPermissions)
                    .ThenInclude(bp => bp.Board)
                .ToListAsync();

            var boards = projectRoles
                .SelectMany(pr => pr.BoardPermissions.Select(bp => bp.Board))
                .Distinct()
                .ToList();

            return boards;
        }

        public async Task AddBoardByProjectIdAsync(int projectId, string boardTitle)
        {
            var currentUserId = await GetCurrentUserIdAsync();

            if (currentUserId == null)
                throw new InvalidOperationException("No signed-in user found.");

            // Fetch the project to ensure it exists
            var project = await _context.Projects
            .Include(p => p.ProjectRoles)
            .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                throw new KeyNotFoundException($"Project with ID {projectId} not found.");

            // Check if the user has a role in this project
            var userRole = project.ProjectRoles.FirstOrDefault(pr => pr.ApplicationUser.Id == currentUserId);
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

        public async Task DeleteBoardByIdAsync(int boardId)
        {
            var board = await _context.Boards
            .Include(b => b.BoardPermissions)
            .FirstOrDefaultAsync(b => b.BoardId == boardId);

            if (board == null)
            {
                throw new InvalidOperationException($"Board with ID {boardId} not found.");
            }

            _context.BoardPermissions.RemoveRange(board.BoardPermissions);

            _context.Boards.Remove(board);

            await _context.SaveChangesAsync();
        }
    }
}
