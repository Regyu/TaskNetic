using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Client.DTO;

namespace TaskNetic.Services.Interfaces
{
    public interface IBoardPermissionService : IRepository<BoardPermission>
    {
        Task UpdateBoardRoleWithParametersAsync(int boardId, string userId, bool canEdit);
        Task RemoveUserFromBoardAsync(int boardId, string userId);
        Task<IEnumerable<BoardMember>> GetBoardMembersAsync(int boardId);
        Task AddUserToBoardAsync(int boardId, string userName, bool canEdit, int projectId);
    }
}
