using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Interfaces
{
    public interface ITodoTaskService : IRepository<TodoTask>
    {
        Task<IEnumerable<TodoTask>> GetTodoTasksByTaskListAsync(TaskList taskList);
        Task AddTodoTaskToTaskListAsync(TaskList taskList, TodoTask todoTask);
        Task DeleteTodoTaskAsync(TodoTask todoTask);
    }
}
