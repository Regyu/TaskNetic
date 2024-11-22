using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Interfaces
{
    public interface IBoardService : IRepository<Board>
    {
        Task<IEnumerable<Board>> GetBoardsByProjectIdForCurrentUserAsync(int projectId);
        Task AddBoardByProjectIdAsync(int projectId, string boardTitle);
        Task DeleteBoardAsync(Board board);
    }
}
