using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskNetic.Client.DTO;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Implementations;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requires authentication to access these endpoints
    public class CardsController : ControllerBase
    {
        private readonly ICardService _cardService;
        private readonly IListService _listService;
        private readonly IRepository<List> _listRepository; // Add a repository for List if needed

        public CardsController(ICardService cardService, IListService listService,IRepository<List> listRepository)
        {
            _cardService = cardService;
            _listService = listService;
            _listRepository = listRepository;
        }

        // GET: api/cards/list/{listId}
        [HttpGet("list/{listId}")]
        public async Task<IActionResult> GetCardsForList(int listId)
        {
            try
            {
                var list = await _listRepository.GetByIdAsync(listId);
                if (list == null)
                    return NotFound(new { message = $"List with ID {listId} not found." });

                var cards = await _cardService.GetCardsForListAsync(list);
                return Ok(cards);
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

        // POST: api/cards/list/{listId}
        [HttpPost("list/{listId}")]
        public async Task<IActionResult> AddCardToList(int listId, [FromBody] string cardTitle)
        {
            try
            {
                var list = await _listRepository.GetByIdAsync(listId);
                if (list == null)
                    return NotFound(new { message = $"List with ID {listId} not found." });

                var existingCards = await _cardService.GetCardsForListAsync(list);

                var maxPosition = existingCards.Any() ? existingCards.Max(c => c.CardPosition) : 0;
                int newPosition = maxPosition + 1;

                var newCard = new Card
                {
                    CardTitle = cardTitle,
                    CardPosition = newPosition,
                    CreatedAt = DateTime.UtcNow
                };

                await _cardService.AddCardToListAsync(list, newCard);
                return Ok(new { message = "Card added successfully." });
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

        // DELETE: api/cards/{cardId}
        [HttpDelete("{cardId}")]
        public async Task<IActionResult> DeleteCard(int cardId)
        {
            try
            {
                var card = await _cardService.GetByIdAsync(cardId);
                if (card == null)
                    return NotFound(new { message = "Card not found." });

                await _cardService.DeleteCardAsync(card);
                return Ok(new { message = "Card deleted successfully." });
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

        [HttpPut("move/{cardId}")]
        public async Task<IActionResult> MoveCard(int cardId, [FromBody] MoveCardRequest request)
        {
            try
            {
                await _listService.MoveCardAsync(cardId, request.SourceListId, request.TargetListId, request.NewPosition);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("update-positions")]
        public async Task<IActionResult> UpdateCardPositions([FromBody] IEnumerable<CardPositionUpdate> updates)
        {
            try
            {
                await _cardService.UpdateCardPositionsAsync(updates);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
