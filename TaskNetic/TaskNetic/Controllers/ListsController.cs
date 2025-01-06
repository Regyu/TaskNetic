using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;
using TaskNetic.Client.DTO;

namespace TaskNetic.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListsController : ControllerBase
    {
        private readonly IListService _listService;
        private readonly IRepository<Board> _boardRepository;

        public ListsController(IListService listService, IRepository<Board> boardRepository)
        {
            _listService = listService;
            _boardRepository = boardRepository;
        }

        // GET: api/lists/board/{boardId}
        [HttpGet("board/{boardId}")]
        public async Task<IActionResult> GetListsForBoard(int boardId)
        {
            try
            {
                var board = await _boardRepository.GetByIdAsync(boardId);
                if (board == null)
                    return NotFound(new { message = $"Board with ID {boardId} not found." });

                var lists = await _listService.GetListsForBoardAsync(board);
                return Ok(lists);
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

        // POST: api/lists/board/{boardId}
        [HttpPost("board/{boardId}")]
        public async Task<IActionResult> AddListToBoard(int boardId, [FromBody] string listName)
        {
            try
            {
                var board = await _boardRepository.GetByIdAsync(boardId);
                if (board == null)
                    return NotFound(new { message = $"Board with ID {boardId} not found." });

                var existingLists = await _listService.GetListsForBoardAsync(board);

                var maxPosition = existingLists.Any() ? existingLists.Max(l => l.Position) : 0;
                int newPosition = maxPosition + 1;

                var newList = new List
                {
                    Title = listName,
                    Position = newPosition
                };

                await _listService.AddListToBoardsAsync(board, newList);
                return Ok(new { message = "List added to board successfully." });
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

        // DELETE: api/lists/{listId}
        [HttpDelete("{listId}")]
        public async Task<IActionResult> DeleteList(int listId)
        {
            try
            {
                var list = await _listService.GetByIdAsync(listId);
                if (list == null)
                    return NotFound(new { message = "List not found." });

                await _listService.DeleteListAsync(list);
                return Ok(new { message = "List deleted successfully." });
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

        // PUT: api/lists/{listId}
        [HttpPut("{listId}/name")]
        public async Task<IActionResult> UpdateListName(int listId, [FromBody] string listName)
        {
            try
            {
                var list = await _listService.GetByIdAsync(listId);
                if (list == null)
                    return NotFound(new { message = "List not found." });

                list.Title = listName;
                await _listService.UpdateAsync(list);
                return Ok(new { message = "List updated successfully." });
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

        [HttpPut("move")]
        public async Task<IActionResult> UpdateLists([FromBody] IEnumerable<MoveListsRequest> listUpdates)
        {
            try
            {
                await _listService.MoveListsAsync(listUpdates);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
