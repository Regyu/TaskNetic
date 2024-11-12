using TaskNetic.Data;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Services.Implementations
{
    public class TodoTaskService : Repository<TodoTask>, ITodoTaskService
    { 
        public TodoTaskService(ApplicationDbContext context) : base(context) {}

        public Task<IEnumerable<TodoTask>> GetTodoTasksByTaskListIdAsync(int taskListId)
        {
            throw new NotImplementedException();
        }
    }
}
