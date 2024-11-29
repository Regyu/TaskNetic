﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
        public async Task<IActionResult> AddListToBoard(int boardId, [FromBody] List list)
        {
            try
            {
                var board = await _boardRepository.GetByIdAsync(boardId);
                if (board == null)
                    return NotFound(new { message = $"Board with ID {boardId} not found." });

                await _listService.AddListToBoardsAsync(board, list);
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
                var list = await _listService.GetByIdAsync(listId); // Ensure GetByIdAsync exists in Repository
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
    }
}
