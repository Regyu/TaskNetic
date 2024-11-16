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
        //private readonly ApplicationDbContext _context;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public BoardService(ApplicationDbContext context, AuthenticationStateProvider authenticationStateProvider) : base(context)
        {
            //_context = context;
            _authenticationStateProvider = authenticationStateProvider;
        }

        private async Task<string?> GetCurrentUserIdAsync()
        {
            // Get the authentication state
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            // Return the user's ID if authenticated
            return user.Identity?.IsAuthenticated == true
                ? user.FindFirstValue(ClaimTypes.NameIdentifier)
                : null;
        }

        public async Task<IEnumerable<Board>> GetBoardsByProjectIdAsync(int projectId)
        {
            var currentUserId = await GetCurrentUserIdAsync();

            if (currentUserId == null)
                throw new InvalidOperationException("No signed-in user found.");

            // Fetch the project and its boards where the current user has access
            var project = await _context.Projects
                .Include(p => p.ProjectBoards) // Include boards related to the project
                .ThenInclude(b => b.BoardUsers) // Include users of each board
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                throw new KeyNotFoundException($"Project with ID {projectId} not found.");

            // Filter boards where the current user is in BoardUsers
            var boards = project.ProjectBoards
                .Where(b => b.BoardUsers.Any(u => u.Id == currentUserId))
                .ToList();

            return boards;
        }

        public async Task AddBoardByProjectIdAsync(int projectId)
        {
            var currentUserId = await GetCurrentUserIdAsync();

            if (currentUserId == null)
                throw new InvalidOperationException("No signed-in user found.");

            // Fetch the project to ensure it exists
            var project = await _context.Projects
                .Include(p => p.ProjectBoards)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                throw new KeyNotFoundException($"Project with ID {projectId} not found.");

            // Create a new board and add the current user
            var newBoard = new Board
            {
                Title = "New Board",
                BoardUsers = new List<ApplicationUser>
                {
                    new ApplicationUser { Id = currentUserId }
                },
                BackgroundId = 0
            };

            project.ProjectBoards.Add(newBoard);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBoardByProjectIdAsync(int projectId, int boardId)
        {
            var currentUserId = await GetCurrentUserIdAsync();

            if (currentUserId == null)
                throw new InvalidOperationException("No signed-in user found.");

            // Fetch the project and its boards
            var project = await _context.Projects
                .Include(p => p.ProjectBoards) // Include related boards
                .ThenInclude(b => b.BoardUsers) // Include users of each board
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                throw new KeyNotFoundException($"Project with ID {projectId} not found.");

            // Find the specific board to delete
            var boardToDelete = project.ProjectBoards.FirstOrDefault(b => b.BoardId == boardId);

            if (boardToDelete == null)
                throw new KeyNotFoundException($"Board with ID {boardId} not found in project with ID {projectId}.");

            // Clear the BoardUsers relation to avoid leftovers
            boardToDelete.BoardUsers.Clear();

            // Remove the specific board
            _context.Boards.Remove(boardToDelete);

            await _context.SaveChangesAsync();
        }
    }
}
