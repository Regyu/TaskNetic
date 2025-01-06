using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Interfaces
{
    public interface ITodoTaskService : IRepository<TodoTask>
    {
        Task<List<TodoTask>> GetTodoTasksByCardAsync(Card card);
        Task AddTodoTaskToCardAsync(Card card, TodoTask todoTask);
        Task DeleteTodoTaskAsync(TodoTask todoTask);
        Task UpdateTodoTaskAsync(TodoTask todoTask);
    }
}
