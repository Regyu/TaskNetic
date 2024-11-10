using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Implementations
{
    public interface ITaskListService : IRepository<TaskList>
    {
        Task<IEnumerable<TaskList>> GetTaskListsByCardIdAsync(int cardId);
    }
}
