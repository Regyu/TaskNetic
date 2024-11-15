using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Interfaces
{
    public interface ITaskListService : IRepository<TaskList>
    {
        Task<IEnumerable<TaskList>> GetTaskListsByCardIdAsync(int cardId);
    }
}
