using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Implementations
{
    public interface IBoardService : IRepository<Board>
    {
        Task<IEnumerable<Board>> GetBoardsByProjectIdAsync(int projectId);
    }
}
