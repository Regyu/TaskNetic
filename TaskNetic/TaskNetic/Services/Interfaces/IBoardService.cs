using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Interfaces
{
    public interface IBoardService : IRepository<Board>
    {
        Task<IEnumerable<Board>> GetBoardsByProjectIdAsync(int projectId);
        Task AddBoardByProjectIdAsync(int projectId);
        Task DeleteBoardByProjectIdAsync(int projectId, int boardId);
    }
}
