using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TodoTasksController : ControllerBase
    {
        private readonly ITodoTaskService _todoTaskService;
        private readonly IRepository<TaskList> _taskListRepository;

        public TodoTasksController(ITodoTaskService todoTaskService, IRepository<TaskList> taskListRepository)
        {
            _todoTaskService = todoTaskService;
            _taskListRepository = taskListRepository;
        }

        // GET: api/todotasks/tasklist/{taskListId}
        [HttpGet("tasklist/{taskListId}")]
        public async Task<IActionResult> GetTodoTasksByTaskList(int taskListId)
        {
            try
            {
                var taskList = await _taskListRepository.GetByIdAsync(taskListId);
                if (taskList == null)
                    return NotFound(new { message = $"TaskList with ID {taskListId} not found." });

                var todoTasks = await _todoTaskService.GetTodoTasksByTaskListAsync(taskList);
                return Ok(todoTasks);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // POST: api/todotasks/tasklist/{taskListId}
        [HttpPost("tasklist/{taskListId}")]
        public async Task<IActionResult> AddTodoTaskToTaskList(int taskListId, [FromBody] TodoTask todoTask)
        {
            try
            {
                var taskList = await _taskListRepository.GetByIdAsync(taskListId);
                if (taskList == null)
                    return NotFound(new { message = $"TaskList with ID {taskListId} not found." });

                await _todoTaskService.AddTodoTaskToTaskListAsync(taskList, todoTask);
                return Ok(new { message = "TodoTask added to TaskList successfully." });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // DELETE: api/todotasks/{todoTaskId}
        [HttpDelete("{todoTaskId}")]
        public async Task<IActionResult> DeleteTodoTask(int todoTaskId)
        {
            try
            {
                var todoTask = await _todoTaskService.GetByIdAsync(todoTaskId); // Ensure GetByIdAsync exists in Repository
                if (todoTask == null)
                    return NotFound(new { message = "TodoTask not found." });

                await _todoTaskService.DeleteTodoTaskAsync(todoTask);
                return Ok(new { message = "TodoTask deleted successfully." });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
