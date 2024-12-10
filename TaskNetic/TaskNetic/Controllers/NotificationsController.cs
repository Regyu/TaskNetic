using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Ensure that the user is authenticated
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // GET: api/Notification
        [HttpGet]
        public async Task<IActionResult> GetUserNotifications()
        {
            try
            {
                var notifications = await _notificationService.GetCurrentUserNotificationsAsync();
                return Ok(notifications);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("User is not authenticated.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // POST: api/Notification
        [HttpPost]
        public async Task<IActionResult> AddNotification([FromBody] AddNotificationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _notificationService.AddNotificationAsync(request.UserId, request.MentionedUserName, request.Message);
                return CreatedAtAction(nameof(GetUserNotifications), null);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


    }

    public class AddNotificationRequest
    {
        public string UserId { get; set; }
        public string MentionedUserName { get; set; }
        public string Message { get; set; }
    }
}
