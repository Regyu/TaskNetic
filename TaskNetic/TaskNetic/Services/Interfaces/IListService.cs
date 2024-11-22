using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Interfaces
{
    public interface IListService : IRepository<List>
    {
        Task<IEnumerable<List>> GetListsForBoardAsync(Board board);
        Task AddListToBoardsAsync(Board board, List list);
        Task DeleteListAsync(List list);
    }
}
