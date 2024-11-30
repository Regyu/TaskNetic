using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BoardPermissionsController : ControllerBase
    {
        private readonly IBoardPermissionService _boardPermissionService;

        public BoardPermissionsController(IBoardPermissionService boardPermissionService)
        {
            _boardPermissionService = boardPermissionService;
        }

        // PUT: api/boardpermissions/{boardId}/users/{userId}
        [HttpPut("{boardId}/users/{userId}")]
        public async Task<IActionResult> UpdateBoardRoleWithParameters(int boardId, string userId, [FromQuery] bool canEdit)
        {
            try
            {
                await _boardPermissionService.UpdateBoardRoleWithParametersAsync(boardId, userId, canEdit);
                return Ok(new { message = "Board role updated successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // DELETE: api/boardpermissions/{boardId}/users/{userId}
        [HttpDelete("{boardId}/users/{userId}")]
        public async Task<IActionResult> RemoveUserFromBoard(int boardId, string userId)
        {
            try
            {
                await _boardPermissionService.RemoveUserFromBoardAsync(boardId, userId);
                return Ok(new { message = "User removed from board successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
