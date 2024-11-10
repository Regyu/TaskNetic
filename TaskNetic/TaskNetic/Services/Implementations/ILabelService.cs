using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Implementations
{
    public interface ILabelService : IRepository<Label>
    {
        Task<IEnumerable<Label>> GetLabelsByBoardIdAsync(int boardId);
    }
}