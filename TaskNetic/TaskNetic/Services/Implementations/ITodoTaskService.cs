using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Implementations
{
    public interface ITodoTaskService : IRepository<TodoTask>
    {
        Task<IEnumerable<TodoTask>> GetTodoTasksByTaskListIdAsync(int taskListId);
    }
}
