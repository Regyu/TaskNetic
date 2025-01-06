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
    [Authorize]
    public class LabelsController : ControllerBase
    {
        private readonly ILabelService _labelService;
        private readonly ICardService _cardService;
        private readonly INotificationService _notificationService;
        private readonly IApplicationUserService _applicationUserService;
        public LabelsController(ILabelService labelService, ICardService cardService, INotificationService notificationService, IApplicationUserService applicationUserService)
        {
            _labelService = labelService;
            _cardService = cardService;
            _notificationService = notificationService;
            _applicationUserService = applicationUserService;
        }

        // GET: api/labels/card/{cardId}
        [HttpGet("card/{cardId}")]
        public async Task<IActionResult> GetLabelsByCard(int cardId)
        {
            try
            {
                var labels = await _labelService.GetLabelsByCardAsync(cardId);
                if (labels == null)
                    return NotFound(new { message = $"Card with ID {cardId} not found." });
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
        // GET: api/labels/board/{boardId}
        [HttpGet("board/{boardId}")]
        public async Task<IActionResult> GetLabelsByBoard(int boardId)
        {
            try
            {
                var labels = await _labelService.GetLabelsByBoardAsync(boardId);
                if (labels == null)
                    return NotFound(new { message = $"Board with ID {boardId} not found." });

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
        public async Task<IActionResult> AddLabelToCard(int cardId, [FromBody] LabelCardRequest request)
        {
            try
            {
                var card = await _cardService.GetCardWithMembersAsync(cardId);
                if (card == null)
                    return NotFound(new { message = $"Card with ID {cardId} not found." });
                var label = await _labelService.GetByIdAsync(request.LabelId);
                if (label == null)
                    return NotFound(new { message = $"Label with ID {request.LabelId} not found." });
                var user = await _applicationUserService.GetUserByIdAsync(request.CurrentUserId);

                foreach (var member in card.CardMembers)
                {
                    if (member.Id != user.Id)
                    {
                        await _notificationService.AddNotificationAsync(member.Id, user.UserName, $"has added label \"{label.LabelName}\" to card \"{card.CardTitle}\".");
                    }
                }
                await _labelService.AddLabelToCardAsync(cardId, label);
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

        // POST: api/labels/board/{boardId}
        [HttpPost("board/{boardId}")]
        public async Task<IActionResult> AddBoardLabel(int boardId, [FromBody] NewBoardLabel label)
        {
            try
            {
                await _labelService.AddBoardLabel(boardId, label);
                return Ok(new { message = "Label added to board successfully." });
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

        //DELETE: api/labels/card/{cardId}/{labelId}
        [HttpDelete("card/{cardId}")]
        public async Task<IActionResult> RemoveLabelFromCard(int cardId, [FromBody] LabelCardRequest request)
        {
            try
            {
                var card = await _cardService.GetCardWithMembersAsync(cardId);
                if (card == null)
                    return NotFound(new { message = $"Card with ID {cardId} not found." });
                var label = await _labelService.GetByIdAsync(request.LabelId);
                if (label == null)
                    return NotFound(new { message = $"Label with ID {request.LabelId} not found." });
                var user = await _applicationUserService.GetUserByIdAsync(request.CurrentUserId);

                foreach (var member in card.CardMembers)
                {
                    if (member.Id != user.Id)
                    {
                        await _notificationService.AddNotificationAsync(member.Id, user.UserName, $"has removed label \"{label.LabelName}\" to card \"{card.CardTitle}\".");
                    }
                }
                await _labelService.RemoveLabelFromCardAsync(cardId, request.LabelId);
                return Ok(new { message = "Label removed from card successfully." });
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

            // PUT: api/labels
            [HttpPut]
        public async Task<IActionResult> UpdateLabel([FromBody] Label label)
        {
            try
            {
                await _labelService.UpdateAsync(label);
                return Ok(new { message = "Label updated successfully." });
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
