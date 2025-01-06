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
    public class FileAttachmentsController : ControllerBase
    {
        private readonly IFileAttachmentService _fileAttachmentService;
        private readonly IRepository<Card> _cardRepository;

        public FileAttachmentsController(IFileAttachmentService fileAttachmentService, IRepository<Card> cardRepository)
        {
            _fileAttachmentService = fileAttachmentService;
            _cardRepository = cardRepository;
        }

        // GET: api/fileattachments/card/{cardId}
        [HttpGet("card/{cardId}")]
        public async Task<IActionResult> GetAttachmentsByCard(int cardId)
        {
            try
            {
                var card = await _cardRepository.GetByIdAsync(cardId);
                if (card == null)
                    return NotFound(new { message = $"Card with ID {cardId} not found." });

                var attachments = await _fileAttachmentService.GetAttachmentsByCardAsync(card);
                return Ok(attachments);
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

        // POST: api/fileattachments/card/{cardId}
        [HttpPost("card/{cardId}")]
        public async Task<IActionResult> AddAttachmentToCard(int cardId, [FromBody] FileAttachment fileAttachment)
        {
            try
            {
                var card = await _cardRepository.GetByIdAsync(cardId);
                if (card == null)
                    return NotFound(new { message = $"Card with ID {cardId} not found." });

                await _fileAttachmentService.AddAttachmentToCardAsync(card, fileAttachment);
                return Ok(new { message = "File attachment added successfully." });
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

        // DELETE: api/fileattachments/{attachmentId}
        [HttpDelete("{attachmentId}")]
        public async Task<IActionResult> DeleteAttachment(int attachmentId)
        {
            try
            {
                var attachment = await _fileAttachmentService.GetByIdAsync(attachmentId); // Ensure GetByIdAsync exists in Repository
                if (attachment == null)
                    return NotFound(new { message = "File attachment not found." });

                await _fileAttachmentService.DeleteAttachmentAsync(attachment);
                return Ok(new { message = "File attachment deleted successfully." });
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
