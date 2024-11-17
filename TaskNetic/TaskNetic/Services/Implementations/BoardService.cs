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
        public async Task<IEnumerable<Board>> GetBoardsForCurrentUserAsync(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project), "Project cannot be null.");
            }

            // Get the current authentication state
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            // Ensure the user is authenticated
            if (user.Identity?.IsAuthenticated != true)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            // Get the user's ID
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new InvalidOperationException("User ID is not available.");
            }

            // Ensure the ProjectBoards navigation property is loaded
            _context.Entry(project).Collection(p => p.ProjectBoards).Load();

            // Filter boards for the current user
            var boards = project.ProjectBoards
                .Where(b => b.BoardUsers.Any(u => u.Id == userId))
                .ToList();

            return boards;
        }

        public async Task AddBoardToProjectAsync(Project project, Board board)
        {

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (project == null)
            {
                throw new ArgumentNullException(nameof(project), "Project cannot be null.");
            }

            if (board == null)
            {
                throw new ArgumentNullException(nameof(board), "Board cannot be null.");
            }

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

            // Ensure the ProjectBoards navigation property is loaded
            _context.Entry(project).Collection(p => p.ProjectBoards).Load();

            // Add the current user to the board's users
            board.BoardUsers.Add(currentUser);

            // Add the new board to the project
            project.ProjectBoards.Add(board);

            await _context.SaveChangesAsync();
        }


        public async Task DeleteBoardAsync(Board board)
        {
            if (board == null)
            {
                throw new ArgumentNullException(nameof(board), "Board cannot be null.");
            }

            // Ensure related BoardUsers are loaded
            _context.Entry(board).Collection(b => b.BoardUsers).Load();

            // Clear the BoardUsers relation to avoid orphaned entries
            board.BoardUsers.Clear();

            // Remove the board
            _context.Boards.Remove(board);

            await _context.SaveChangesAsync();
        }

    }
}
