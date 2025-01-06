using TaskNetic.Data.Repository;
using TaskNetic.Data;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TaskNetic.Client.DTO;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore.Internal;
using static TaskNetic.Components.Layout.EditProjectModal;

namespace TaskNetic.Services.Implementations
{
    public class BoardPermissionService : Repository<BoardPermission>, IBoardPermissionService
    {
        private readonly ApplicationUserService _applicationUserService;
        private readonly NotificationService _notificationService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public BoardPermissionService(ApplicationDbContext context, AuthenticationStateProvider authenticationStateProvider, IDbContextFactory<ApplicationDbContext> dbContextFactory) : base(context)
        {
            _applicationUserService = new ApplicationUserService(context, authenticationStateProvider);
            _notificationService = new NotificationService(dbContextFactory, authenticationStateProvider);
            _authenticationStateProvider = authenticationStateProvider;
        }
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

        public async Task RemoveUserFromBoardAsync(int boardId, string userId, string currentUserId)
        {
            var boardPermission = await _context.BoardPermissions
                .FirstOrDefaultAsync(bp => bp.Board.BoardId == boardId && bp.Role.ApplicationUser.Id == userId);

            var boardPermissions = await _context.BoardPermissions
                .Where(bp => bp.Board.BoardId == boardId)
                .Select(bp => new
                {
                    bp.Board,
                    bp.Role,
                    ApplicationUser = bp.Role.ApplicationUser
                })
                .ToListAsync();


            var currentUser = await _applicationUserService.GetUserByIdAsync(currentUserId);

            var user = await _applicationUserService.GetUserByIdAsync(userId);

            if (boardPermission != null)
            {
                _context.Remove(boardPermission);

                foreach (var permission in boardPermissions)
                {
                    if (currentUserId != permission.Role.ApplicationUser.Id && user.Id != permission.Role.ApplicationUser.Id)
                    {
                        await _notificationService.AddNotificationAsync(permission.Role.ApplicationUser.Id, user.UserName, $"has been removed from the board \"{permission.Board.Title}\".");
                    }
                }
                await _notificationService.AddNotificationAsync(user.Id, currentUser.UserName, $"has removed you from the board \"{boardPermission.Board.Title}\".");

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

        public async Task AddUserToBoardAsync(int boardId, string userName, bool canEdit, int projectId, string currentUserId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            var board = await _context.Boards.FirstOrDefaultAsync(b => b.BoardId == boardId);
            if (board == null)
                throw new InvalidOperationException("Board not found.");

            var projectRole = await _context.ProjectRoles
                .Include(pr => pr.Project)
                .FirstOrDefaultAsync(pr => pr.ApplicationUser.Id == user.Id && pr.Project.Id == projectId);

            if (projectRole == null)
                throw new InvalidOperationException("User does not have a project role for this board's project.");

            var existingPermission = await _context.BoardPermissions
                .FirstOrDefaultAsync(bp => bp.Board.BoardId == boardId && bp.Role.Id == projectRole.Id);

            if (existingPermission != null)
                throw new InvalidOperationException("This user already has permissions for this board.");

            var boardPermissions = await _context.BoardPermissions
                .Include(bp => bp.Role)
                .ThenInclude(r => r.ApplicationUser)
                .Where(bp => bp.Board.BoardId == boardId)
                .ToListAsync();

            var currentUser = await _applicationUserService.GetUserByIdAsync(currentUserId);

            foreach (var permission in boardPermissions)
            {
                if (currentUserId != permission.Role.ApplicationUser.Id)
                {
                    await _notificationService.AddNotificationAsync(permission.Role.ApplicationUser.Id, user.UserName, $"has been added to the board \"{board.Title}\".");
                }
            }
            await _notificationService.AddNotificationAsync(user.Id, currentUser.UserName, $"has added you to the board \"{board.Title}\".");

            var boardPermission = new BoardPermission
            {
                Board = board,
                Role = projectRole,
                CanEdit = canEdit
            };

            await _context.BoardPermissions.AddAsync(boardPermission);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CanUserEditBoardAsync(int boardId, string userId)
        {
            var boardPermissions = await _context.BoardPermissions
                .Include(bp => bp.Role)
                .ThenInclude(role => role.ApplicationUser)
                .FirstOrDefaultAsync(bp => bp.Board.BoardId == boardId && bp.Role.ApplicationUser.Id == userId);

            return boardPermissions.CanEdit;
        }
    }
}
