using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Interfaces
{
    public interface IBoardService : IRepository<Board>
    {
        Task<IEnumerable<Board>> GetBoardsByProjectAndUserIdAsync(int projectId, string userId);
        Task AddBoardByProjectAndUserIdAsync(int projectId, string userId, string boardTitle);
        Task DeleteBoardAsync(Board board);
        Task UpdateBoardAsync(int boardId, string boardName);
    }
}
