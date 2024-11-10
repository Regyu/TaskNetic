using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Implementations
{
    public interface IListService : IRepository<List>
    {
        Task<IEnumerable<List>> GetListsByBoardIdAsync(int boardId);
    }
}
