using Azure.Core;
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
    public class CardsController : ControllerBase
    {
        private readonly ICardService _cardService;
        private readonly IListService _listService;
        private readonly IRepository<List> _listRepository;
        private readonly IApplicationUserService _aplicationUserService;

        public CardsController(ICardService cardService, IListService listService,IRepository<List> listRepository, IApplicationUserService aplicationUserService)
        {
            _cardService = cardService;
            _listService = listService;
            _listRepository = listRepository;
            _aplicationUserService = aplicationUserService;
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

        // GET: api/cards/{cardId}/info
        [HttpGet("{cardId}/info")]
        public async Task<IActionResult> GetFullCardInfo(int cardId)
        {
            var card = await _cardService.GetFullCardInfoAsync(cardId);

            if (card == null)
                return NotFound(new { message = "Card not found" });

            return Ok(card);
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

        [HttpPut("{cardId}/title")]
        public async Task<IActionResult> UpdateCardTitle(int cardId, [FromBody] string title)
        {
            var card = await _cardService.GetByIdAsync(cardId);

            if (card == null)
                return NotFound(new { message = "Card not found" });

            card.CardTitle = title;
            await _cardService.UpdateAsync(card);

            return Ok(new { message = "Card title updated successfully" });
        }

        [HttpGet("{cardId}/members")]
        public async Task<IActionResult> GetCardMembers(int cardId)
        {
            var members = await _cardService.GetCardMembersAsync(cardId);
            if (members == null)
                return NotFound(new { message = "Card not found" });
            return Ok(members);
        }

        [HttpPost("{cardId}/members")]
        public async Task<IActionResult> AddMemberToCard(int cardId, [FromBody] string userId)
        {
            var card = await _cardService.GetFullCardInfoAsync(cardId);

            if (card == null)
                return NotFound(new { message = "Card not found" });

            if (card.CardMembers.Any(member => member.Id == userId))
                return BadRequest(new { message = "User is already a member of this card" });

            var user = await _aplicationUserService.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound(new { message = "User not found" });
            card.CardMembers.Add(user);

            await _cardService.UpdateAsync(card);

            return Ok(new { message = "Member added successfully" });
        }

        [HttpDelete("{cardId}/members/{userId}")]
        public async Task<IActionResult> RemoveMemberFromCard(int cardId, string userId)
        {
            var card = await _cardService.GetFullCardInfoAsync(cardId);

            if (card == null)
                return NotFound(new { message = "Card not found" });

            var memberToRemove = card.CardMembers.FirstOrDefault(member => member.Id == userId);
            if (memberToRemove == null)
                return BadRequest(new { message = "User is not a member of this card" });

            card.CardMembers.Remove(memberToRemove);

            await _cardService.UpdateAsync(card);

            return Ok(new { message = "Member removed successfully" });
        }

        [HttpPut("{cardId}/description")]
        public async Task<IActionResult> UpdateCardDescription(int cardId, [FromBody] string description)
        {
            var card = await _cardService.GetByIdAsync(cardId);

            if (card == null)
                return NotFound(new { message = "Card not found" });

            card.CardDescription = description;
            await _cardService.UpdateAsync(card);

            return Ok(new { message = "Card description updated successfully" });
        }

        [HttpPut("{cardId}/due-date")]
        public async Task<IActionResult> UpdateCardDueDate(int cardId, [FromBody] DateTime? dueDate)
        {
            var card = await _cardService.GetByIdAsync(cardId);

            if (card == null)
                return NotFound(new { message = "Card not found" });

            card.DueDate = dueDate;
            await _cardService.UpdateAsync(card);

            return Ok(new { message = "Card due date updated successfully" });
        }
    }
}
