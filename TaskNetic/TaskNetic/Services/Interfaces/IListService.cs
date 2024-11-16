using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Interfaces
{
    public interface IListService : IRepository<List>
    {
        Task<IEnumerable<List>> GetListsByBoardIdAsync(int boardId);
        Task AddListByBoardIdAsync(int boardId, string listTitle);
        Task DeleteListByBoardIdAsync(int boardId, int listId);
    }
}
