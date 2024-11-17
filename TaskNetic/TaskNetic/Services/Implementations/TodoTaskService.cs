using TaskNetic.Data;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Services.Implementations
{
    public class TodoTaskService : Repository<TodoTask>, ITodoTaskService
    { 
        public TodoTaskService(ApplicationDbContext context) : base(context) {}

        public async Task<IEnumerable<TodoTask>> GetTodoTasksByTaskListAsync(TaskList taskList)
        {
            if (taskList == null)
            {
                throw new ArgumentNullException(nameof(taskList), "TaskList cannot be null.");
            }

            await _context.Entry(taskList).Collection(c => c.TodoTasks).LoadAsync();

            return taskList.TodoTasks.AsEnumerable();
        }

        public async Task AddTodoTaskToTaskListAsync(TaskList taskList, TodoTask todoTask)
        {

            if (taskList == null)
            {
                throw new ArgumentNullException(nameof(taskList), "TaskList cannot be null.");
            }

            if (todoTask == null)
            {
                throw new ArgumentNullException(nameof(todoTask), "TodoTask cannot be null.");
            }


            taskList.TodoTasks.Add(todoTask);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteTodoTaskAsync(TodoTask todoTask)
        {
            if (todoTask == null)
            {
                throw new ArgumentNullException(nameof(TodoTask), "TodoTask cannot be null.");
            }

            _context.TodoTasks.Remove(todoTask);

            await _context.SaveChangesAsync();
        }
    }
}
