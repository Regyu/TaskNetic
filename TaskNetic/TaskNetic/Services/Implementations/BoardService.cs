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

        public BoardService(ApplicationDbContext context, AuthenticationStateProvider authenticationStateProvider) : base(context)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }
        public async Task<IEnumerable<Board>> GetBoardsForCurrentUserAsync(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project), "Project cannot be null.");
            }

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

            //_context.Entry(project).Collection(p => p.ProjectBoards).Load();
            await _context.Entry(project).Collection(c => c.ProjectBoards).LoadAsync();

            /*
            var boards = project.ProjectBoards
                .Where(b => b.BoardUsers.Any(u => u.Id == userId))
                .ToList();
            return boards;*/

            return project.ProjectBoards
                .Where(b => b.BoardUsers.Any(u => u.Id == userId))
                .AsEnumerable();
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

            _context.Entry(project).Collection(p => p.ProjectBoards).Load();

            board.BoardUsers.Add(currentUser);

            project.ProjectBoards.Add(board);

            await _context.SaveChangesAsync();
        }


        public async Task DeleteBoardAsync(Board board)
        {
            if (board == null)
            {
                throw new ArgumentNullException(nameof(board), "Board cannot be null.");
            }

            _context.Entry(board).Collection(b => b.BoardUsers).Load();

            board.BoardUsers.Clear();

            _context.Boards.Remove(board);

            await _context.SaveChangesAsync();
        }

    }
}
