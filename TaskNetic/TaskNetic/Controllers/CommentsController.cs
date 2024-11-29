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
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IRepository<Card> _cardRepository;

        public CommentsController(ICommentService commentService, IRepository<Card> cardRepository)
        {
            _commentService = commentService;
            _cardRepository = cardRepository;
        }

        // GET: api/comments/card/{cardId}
        [HttpGet("card/{cardId}")]
        public async Task<IActionResult> GetCommentsByCard(int cardId)
        {
            try
            {
                var card = await _cardRepository.GetByIdAsync(cardId);
                if (card == null)
                    return NotFound(new { message = $"Card with ID {cardId} not found." });

                var comments = await _commentService.GetCommentsByCardAsync(card);
                return Ok(comments);
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

        // POST: api/comments/card/{cardId}
        [HttpPost("card/{cardId}")]
        public async Task<IActionResult> AddCommentToCard(int cardId, [FromBody] Comment comment)
        {
            try
            {
                var card = await _cardRepository.GetByIdAsync(cardId);
                if (card == null)
                    return NotFound(new { message = $"Card with ID {cardId} not found." });

                await _commentService.AddCommentToCardAsync(card, comment);
                return Ok(new { message = "Comment added successfully." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
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

        // DELETE: api/comments/{commentId}
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            try
            {
                var comment = await _commentService.GetByIdAsync(commentId); // Ensure GetByIdAsync is implemented in Repository
                if (comment == null)
                    return NotFound(new { message = "Comment not found." });

                await _commentService.DeleteCommentAsync(comment);
                return Ok(new { message = "Comment deleted successfully." });
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
