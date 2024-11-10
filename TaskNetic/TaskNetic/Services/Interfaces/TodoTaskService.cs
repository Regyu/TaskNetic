using TaskNetic.Data;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Implementations;

namespace TaskNetic.Services.Interfaces
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
