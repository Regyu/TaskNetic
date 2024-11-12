using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Interfaces
{
    public interface ILabelService : IRepository<Label>
    {
        Task<IEnumerable<Label>> GetLabelsByBoardIdAsync(int boardId);
    }
}