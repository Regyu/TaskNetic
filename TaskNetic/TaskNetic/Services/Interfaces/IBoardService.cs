using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Interfaces
{
    public interface IBoardService : IRepository<Board>
    {
        Task<IEnumerable<Board>> GetBoardsForCurrentUserAsync(Project project);
        Task AddBoardToProjectAsync(Project project, Board board);
        Task DeleteBoardAsync(Board board);
    }
}
