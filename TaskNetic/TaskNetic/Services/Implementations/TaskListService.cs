using TaskNetic.Data;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Services.Implementations
{
    public class TaskListService : Repository<TaskList>, ITaskListService
    {
        public TaskListService(ApplicationDbContext context) : base(context) { }

        public Task<IEnumerable<TaskList>> GetTaskListsByCardIdAsync(int cardId)
        {
            throw new NotImplementedException();
        }
    }
}
