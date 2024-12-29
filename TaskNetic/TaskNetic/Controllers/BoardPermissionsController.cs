using Microsoft.AspNetCore.Mvc;
using TaskNetic.Models;
using TaskNetic.Services.Implementations;
using TaskNetic.Services.Interfaces;
using TaskNetic.Client.DTO;

namespace TaskNetic.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoardPermissionsController : ControllerBase
    {
        private readonly IBoardPermissionService _boardPermissionService;

        public BoardPermissionsController(IBoardPermissionService boardPermissionService)
        {
            _boardPermissionService = boardPermissionService;
        }

        // GET: api/boardpermissions/{boardId}/team
        [HttpGet("{boardId}/team")]
        public async Task<IActionResult> GetTeamMembers(int boardId)
        {
            try
            {
                var teamMembers = await _boardPermissionService.GetBoardMembersAsync(boardId);
                return Ok(teamMembers);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/boardpermissions/{boardId}/user/{userId}
        [HttpPut("{boardId}/user/{userId}")]
        public async Task<IActionResult> UpdateBoardRoleWithParameters(int boardId, string userId, [FromBody] bool canEdit)
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

        // POST: api/boardpermissions
        [HttpPost]
        public async Task<IActionResult> AddTeamMember([FromBody] NewBoardMember request)
        {
            try
            {
                await _boardPermissionService.AddUserToBoardAsync(request.boardId, request.userName, request.canEdit, request.projectId, request.currentUserId);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/boardpermissions/{boardId}/user/{userId}
        //[HttpDelete("{boardId}/user/{userId}")]
        [HttpDelete]
        public async Task<IActionResult> RemoveUserFromBoard([FromBody] RemoveBoardMember request)//int boardId, string userId, string currentUserId)
        {
            try
            {
                await _boardPermissionService.RemoveUserFromBoardAsync(request.boardId, request.userId, request.currentUserId);
                return Ok(new { message = "User removed from board successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("can-edit/{boardId}/{userId}")]
        public async Task<IActionResult> IsUserAdmin(int boardId, string userId)
        {
            try
            {
                var canEdit = await _boardPermissionService.CanUserEditBoardAsync(boardId, userId);
                return Ok(canEdit);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
