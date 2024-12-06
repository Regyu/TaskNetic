using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoardsController : ControllerBase
    {
        private readonly IBoardService _boardService;

        public BoardsController(IBoardService boardService)
        {
            _boardService = boardService;
        }

        [HttpGet("{projectId}/{userId}")]
        public async Task<IActionResult> GetBoardsByProjectId(int projectId, string userId)
        {
            try
            {
                var boards = await _boardService.GetBoardsByProjectAndUserIdAsync(projectId, userId);
                return Ok(boards);
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

        [HttpPost("{projectId}/{userId}")]
        public async Task<IActionResult> AddBoard(int projectId, string userId, [FromBody] string boardTitle)
        {
            try
            {
                await _boardService.AddBoardByProjectAndUserIdAsync(projectId, userId, boardTitle);
                return Ok(new { message = "Board added successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{boardId}")]
        public async Task<IActionResult> DeleteBoard(int boardId)
        {
            try
            {
                // Assuming you fetch the board object by ID before deleting
                var board = await _boardService.GetByIdAsync(boardId); // You may need a method to fetch by ID
                if (board == null)
                    return NotFound(new { message = "Board not found." });

                await _boardService.DeleteBoardAsync(board);
                return Ok(new { message = "Board deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
