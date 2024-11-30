using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Interfaces
{
    public interface IBoardPermissionService : IRepository<BoardPermission>
    {
        Task UpdateBoardRoleWithParametersAsync(int boardId, string userId, bool canEdit);
        Task RemoveUserFromBoardAsync(int boardId, string userId);
    }
}
