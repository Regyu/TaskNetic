using Microsoft.EntityFrameworkCore;
using TaskNetic.Data.Repository;
using TaskNetic.Data;
using TaskNetic.Models;
using TaskNetic.Services.Implementations;

namespace TaskNetic.Services.Interfaces
{
    public class BoardService : Repository<Board>, IBoardService
    {
        public BoardService(ApplicationDbContext context) : base(context) { }

       public Task<IEnumerable<Board>> GetBoardsByProjectIdAsync(int projectId)
        {
            throw new NotImplementedException();
        }
    }
}
