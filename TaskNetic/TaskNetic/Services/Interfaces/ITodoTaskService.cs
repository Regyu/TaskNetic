using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Interfaces
{
    public interface ITodoTaskService : IRepository<TodoTask>
    {
        Task<IEnumerable<TodoTask>> GetTodoTasksByTaskListIdAsync(int taskListId);
    }
}
