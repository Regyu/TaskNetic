using TaskNetic.Data;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Implementations;

namespace TaskNetic.Services.Interfaces
{
    public class ListService : Repository<List>, IListService
    {
        public ListService(ApplicationDbContext context) : base(context) { }

        public Task<IEnumerable<List>> GetListsByBoardIdAsync(int boardId)
        {
            throw new NotImplementedException();
        }
    }
}
