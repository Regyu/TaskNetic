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
    public class LabelsController : ControllerBase
    {
        private readonly ILabelService _labelService;
        private readonly IRepository<Card> _cardRepository;

        public LabelsController(ILabelService labelService, IRepository<Card> cardRepository)
        {
            _labelService = labelService;
            _cardRepository = cardRepository;
        }

        // GET: api/labels/card/{cardId}
        [HttpGet("card/{cardId}")]
        public async Task<IActionResult> GetLabelsByCard(int cardId)
        {
            try
            {
                var card = await _cardRepository.GetByIdAsync(cardId);
                if (card == null)
                    return NotFound(new { message = $"Card with ID {cardId} not found." });

                var labels = await _labelService.GetLabelsByCardAsync(card);
                return Ok(labels);
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

        // POST: api/labels/card/{cardId}
        [HttpPost("card/{cardId}")]
        public async Task<IActionResult> AddLabelToCard(int cardId, [FromBody] Label label)
        {
            try
            {
                var card = await _cardRepository.GetByIdAsync(cardId);
                if (card == null)
                    return NotFound(new { message = $"Card with ID {cardId} not found." });

                await _labelService.AddLabelToCardAsync(card, label);
                return Ok(new { message = "Label added to card successfully." });
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

        // DELETE: api/labels/{labelId}
        [HttpDelete("{labelId}")]
        public async Task<IActionResult> DeleteLabel(int labelId)
        {
            try
            {
                var label = await _labelService.GetByIdAsync(labelId);
                if (label == null)
                    return NotFound(new { message = "Label not found." });

                await _labelService.DeleteLabelAsync(label);
                return Ok(new { message = "Label deleted successfully." });
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
