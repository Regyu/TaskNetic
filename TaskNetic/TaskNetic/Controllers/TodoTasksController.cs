using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskNetic.Client.DTO;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Implementations;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoTasksController : ControllerBase
    {
        private readonly ITodoTaskService _todoTaskService;
        private readonly INotificationService _notificationService;
        private readonly IRepository<Card> _cardRepository;
        private readonly ICardService _cardService;
        private readonly IApplicationUserService _applicationUserService;

        public TodoTasksController(ITodoTaskService todoTaskService, INotificationService notificationService, IRepository<Card> cardRepository, ICardService cardService, IApplicationUserService applicationUserService)
        {
            _todoTaskService = todoTaskService;
            _cardRepository = cardRepository;
            _cardService = cardService;
            _notificationService = notificationService;
            _applicationUserService = applicationUserService;
        }

        // GET: api/todotasks/card/{cardId}
        [HttpGet("card/{cardId}")]
        public async Task<IActionResult> GetTodoTasksByCard(int cardId)
        {
            try
            {
                var card = await _cardRepository.GetByIdAsync(cardId);
                if (card == null)
                    return NotFound(new { message = $"Card with ID {cardId} not found." });

                var todoTasks = await _todoTaskService.GetTodoTasksByCardAsync(card);
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

        // POST: api/todotasks/card/{cardId}
        [HttpPost("card/{cardId}")]
        public async Task<IActionResult> AddTodoTaskToCard(int cardId, [FromBody] NewUserString todoTask)
        {
            try
            {
                var card = await _cardService.GetCardWithMembersAsync(cardId);
                if (card == null)
                    return NotFound(new { message = $"Card with ID {cardId} not found." });

                TodoTask newTask = new TodoTask { TaskName = todoTask.Text, Card = card, TaskFinished = false };
                await _todoTaskService.AddTodoTaskToCardAsync(card, newTask);

                var user = await _applicationUserService.GetUserByIdAsync(todoTask.CurrentUserId);

                foreach (var member in card.CardMembers)
                {
                    if (member.Id != user?.Id)
                    {
                        await _notificationService.AddNotificationAsync(member.Id, user.UserName, $"has added a task \"{todoTask.Text}\" to card \"{card.CardTitle}\".");
                    }
                }

                return Ok(new { message = "TodoTask added to Card successfully." });
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
                var todoTask = await _todoTaskService.GetByIdAsync(todoTaskId);
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

        [HttpPut]
        public async Task<IActionResult> UpdateTodoTask([FromBody] TodoTask newTask)
        {
            try
            {
                await _todoTaskService.UpdateTodoTaskAsync(newTask);
                return Ok(new { message = "TodoTask updated successfullly" });
            }catch(Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
