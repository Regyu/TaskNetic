using TaskNetic.Data;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Services.Implementations
{
    public class TodoTaskService : Repository<TodoTask>, ITodoTaskService
    { 
        public TodoTaskService(ApplicationDbContext context) : base(context) {}

        public async Task<List<TodoTask>> GetTodoTasksByCardAsync(Card card)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card), "Card cannot be null.");
            }

            await _context.Entry(card).Collection(c => c.TodoTasks).LoadAsync();

            return card.TodoTasks.ToList();
        }

        public async Task AddTodoTaskToCardAsync(Card card, TodoTask todoTask)
        {

            if (card == null)
            {
                throw new ArgumentNullException(nameof(card), "Card cannot be null.");
            }

            if (todoTask == null)
            {
                throw new ArgumentNullException(nameof(todoTask), "TodoTask cannot be null.");
            }


            card.TodoTasks.Add(todoTask);

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

        public async Task UpdateTodoTaskAsync(TodoTask todoTask)
        {
            if (todoTask == null)
            {
                throw new ArgumentNullException(nameof(TodoTask), "TodoTask cannot be null.");
            }
            var task = _context.Find<TodoTask>(todoTask.Id);
            task.TaskName = todoTask.TaskName;
            task.TaskFinished = todoTask.TaskFinished;
            _context.TodoTasks.Update(task);

            await _context.SaveChangesAsync();
        }
    }
}
