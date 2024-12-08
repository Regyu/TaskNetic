using TaskNetic.Data.Repository;
using TaskNetic.Data;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TaskNetic.Client.DTO;

namespace TaskNetic.Services.Implementations
{
    public class BoardPermissionService : Repository<BoardPermission>, IBoardPermissionService
    {
        public BoardPermissionService(ApplicationDbContext context) : base(context) { }
        public Task<IEnumerable<ProjectRole>> GetBoardRoleByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateBoardRoleWithParametersAsync(int boardId, string userId, bool canEdit)
        {
            
            var boardPermission = await _context.BoardPermissions
                .FirstOrDefaultAsync(bp => bp.Board.BoardId == boardId && bp.Role.ApplicationUser.Id == userId);

            if (boardPermission == null)
            {
                throw new InvalidOperationException("Project role not found.");
            }

            boardPermission.CanEdit = canEdit;

            await _context.SaveChangesAsync();
        }

        public async Task RemoveUserFromBoardAsync(int boardId, string userId)
        {
            var boardPermission = await _context.BoardPermissions
                .Include(b => b)
                .FirstOrDefaultAsync(b => b.Id == boardId && b.Role.ApplicationUser.Id == userId);

            if (boardPermission != null)
            {
                _context.Remove(boardPermission);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<BoardMember>> GetBoardMembersAsync(int boardId)
        {
            return await _context.BoardPermissions
                .Where(bp => bp.Board.BoardId == boardId)
                .Select(bp => new BoardMember
                {
                    Id = bp.Role.ApplicationUser.Id,
                    Name = bp.Role.ApplicationUser.UserName,
                    CanEdit = bp.CanEdit
                })
                .ToListAsync();
        }

        public async Task AddUserToBoardAsync(int boardId, string userName, bool canEdit, int projectId)
        {
            // Fetch the user by userId
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            // Fetch the board by boardId
            var board = await _context.Boards.FirstOrDefaultAsync(b => b.BoardId == boardId);
            if (board == null)
                throw new InvalidOperationException("Board not found.");

            // Fetch the project role associated with the user and the project of the board
            var projectRole = await _context.ProjectRoles
                .Include(pr => pr.Project)
                .FirstOrDefaultAsync(pr => pr.ApplicationUser.Id == user.Id && pr.Project.Id == projectId);

            if (projectRole == null)
                throw new InvalidOperationException("User does not have a project role for this board's project.");

            // Check if a BoardPermission already exists for this user and board
            var existingPermission = await _context.BoardPermissions
                .FirstOrDefaultAsync(bp => bp.Board.BoardId == boardId && bp.Role.Id == projectRole.Id);

            if (existingPermission != null)
                throw new InvalidOperationException("This user already has permissions for this board.");

            // Create a new BoardPermission entry
            var boardPermission = new BoardPermission
            {
                Board = board,
                Role = projectRole,
                CanEdit = canEdit
            };

            // Save the new permission to the database
            await _context.BoardPermissions.AddAsync(boardPermission);
            await _context.SaveChangesAsync();
        }
    }
}
