using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskNetic.Client.DTO;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IRepository<Card> _cardRepository;
        private readonly IApplicationUserService _userService;

        public CommentsController(ICommentService commentService, IRepository<Card> cardRepository, IApplicationUserService userService)
        {
            _commentService = commentService;
            _cardRepository = cardRepository;
            _userService = userService;
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
        public async Task<IActionResult> AddCommentToCard(int cardId, [FromBody] NewCommentRequest comment)
        {
            try
            {
                var card = await _cardRepository.GetByIdAsync(cardId);
                if (card == null)
                    return NotFound(new { message = $"Card with ID {cardId} not found." });

                var user = await _userService.GetUserByIdAsync(comment.userId);
                if (user == null)
                    return NotFound(new { message = $"User with ID {comment.userId} not found." });

                var newComment = new Comment
                {
                    CommentText = comment.Comment,
                    User = user,
                    Time = comment.creationDate,
                };

                await _commentService.AddCommentToCardAsync(card, newComment);
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

        // PUT: api/comments/{commentId}
        [HttpPut("{commentId}")]
        public async Task<IActionResult> UpdateComment(int commentId, [FromBody] string commentText)
        {
            try
            {
                var commentToUpdate = await _commentService.GetByIdAsync(commentId);
                if (commentToUpdate == null)
                    return NotFound(new { message = "Comment not found." });

                commentToUpdate.CommentText = commentText;
                await _commentService.UpdateAsync(commentToUpdate);
                return Ok(new { message = "Comment updated successfully." });
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
