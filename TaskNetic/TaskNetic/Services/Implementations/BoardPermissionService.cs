using TaskNetic.Data.Repository;
using TaskNetic.Data;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
                .FirstOrDefaultAsync(b => b.Id == boardId && b.Role.ApplicationUser.Id == userId);

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
    }
}
