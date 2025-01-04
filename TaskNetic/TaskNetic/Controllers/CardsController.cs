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
        private readonly INotificationService _notificationService;

        public CardsController(ICardService cardService, IListService listService,IRepository<List> listRepository, IApplicationUserService aplicationUserService, INotificationService notificationService)
        {
            _cardService = cardService;
            _listService = listService;
            _listRepository = listRepository;
            _aplicationUserService = aplicationUserService;
            _notificationService = notificationService;
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
        [HttpDelete("{cardId}/{userId}")]
        public async Task<IActionResult> DeleteCard(int cardId, string userId)
        {
            try
            {
                var card = await _cardService.GetCardWithMembersAsync(cardId);
                if (card == null)
                    return NotFound(new { message = "Card not found." });

                var user = await _aplicationUserService.GetUserByIdAsync(userId);
                if (user == null)
                    return NotFound(new { message = "User not found." });

                foreach (var member in card.CardMembers)
                {
                    if (member.Id == userId)
                    {
                        await _notificationService.AddNotificationAsync(member.Id, user.UserName, $"has removed card \"{card.CardTitle}\".");
                    }
                }

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
                var card = await _cardService.GetCardWithMembersAsync(cardId);
                var list = await _listService.GetByIdAsync(request.TargetListId);
                var user = await _aplicationUserService.GetUserByIdAsync(request.CurrentUserId);

                foreach (var member in card.CardMembers)
                {
                    if (member.Id != user?.Id)
                    {
                        await _notificationService.AddNotificationAsync(member.Id, user.UserName, $"has moved card \"{card.CardTitle}\" to list \"{list.Title}\".");
                    }
                }

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
        public async Task<IActionResult> UpdateCardTitle(int cardId, [FromBody] NewUserString title)
        {
            var card = await _cardService.GetCardWithMembersAsync(cardId);

            if (card == null)
                return NotFound(new { message = "Card not found" });

            var user = await _aplicationUserService.GetUserByIdAsync(title.CurrentUserId);

            foreach (var member in card.CardMembers)
            {
                if (member.Id != user.Id)
                {
                    await _notificationService.AddNotificationAsync(member.Id, user.UserName, $"has changed name of card \"{card.CardTitle}\" to \"{title.Text}\".");
                }
            }

            card.CardTitle = title.Text;
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
        public async Task<IActionResult> AddMemberToCard(int cardId, [FromBody] NewUserString userString)
        {
            var card = await _cardService.GetFullCardInfoAsync(cardId);
            var userId = userString.Text;
            var currentUser = await _aplicationUserService.GetUserByIdAsync(userString.CurrentUserId);

            if (card == null)
                return NotFound(new { message = "Card not found" });

            if (card.CardMembers.Any(member => member.Id == userId))
                return BadRequest(new { message = "User is already a member of this card" });

            var user = await _aplicationUserService.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound(new { message = "User not found" });
            card.CardMembers.Add(user);

            foreach (var member in card.CardMembers)
            {
                if (member.Id != userId && member.Id != currentUser?.Id)
                {
                    await _notificationService.AddNotificationAsync(member.Id, user.UserName, $"has been added to card \"{card.CardTitle}\".");
                }
            }
            await _notificationService.AddNotificationAsync(userId, currentUser.UserName, $"has added you to card \"{card.CardTitle}\".");

            await _cardService.UpdateAsync(card);

            return Ok(new { message = "Member added successfully" });
        }

        [HttpDelete("{cardId}/members")]
        public async Task<IActionResult> RemoveMemberFromCard(int cardId, [FromBody] NewUserString userString)
        {
            var card = await _cardService.GetFullCardInfoAsync(cardId);
            var userId = userString.Text;

            var currentUser = await _aplicationUserService.GetUserByIdAsync(userString.CurrentUserId);

            if (card == null)
                return NotFound(new { message = "Card not found" });

            var memberToRemove = card.CardMembers.FirstOrDefault(member => member.Id == userId);
            if (memberToRemove == null)
                return BadRequest(new { message = "User is not a member of this card" });

            card.CardMembers.Remove(memberToRemove);

            foreach (var member in card.CardMembers)
            {
                if (member.Id != userId && member.Id != currentUser?.Id)
                {
                    await _notificationService.AddNotificationAsync(member.Id, memberToRemove.UserName, $"has been removed from card \"{card.CardTitle}\".");
                }
            }
            await _notificationService.AddNotificationAsync(userId, currentUser.UserName, $"has removed you from card \"{card.CardTitle}\".");

            await _cardService.UpdateAsync(card);

            return Ok(new { message = "Member removed successfully" });
        }

        [HttpPut("{cardId}/description")]
        public async Task<IActionResult> UpdateCardDescription(int cardId, [FromBody] NewUserString description)
        {
            var card = await _cardService.GetCardWithMembersAsync(cardId);

            if (card == null)
                return NotFound(new { message = "Card not found" });

            card.CardDescription = description.Text;
            await _cardService.UpdateAsync(card);

            var user = await _aplicationUserService.GetUserByIdAsync(description.CurrentUserId);

            foreach (var member in card.CardMembers)
            {
                if (member.Id != user.Id)
                {
                    await _notificationService.AddNotificationAsync(member.Id, user.UserName, $"has changed description of card \"{card.CardTitle}\".");
                }
            }

            return Ok(new { message = "Card description updated successfully" });
        }

        [HttpPut("{cardId}/due-date")]
        public async Task<IActionResult> UpdateCardDueDate(int cardId, [FromBody] DateTime? dueDate)
        {
            var card = await _cardService.GetByIdAsync(cardId);

            if (card == null)
                return NotFound(new { message = "Card not found" });

            card.DueDate = dueDate.Value.ToUniversalTime();
            await _cardService.UpdateAsync(card);

            return Ok(new { message = "Card due date updated successfully" });
        }

        [HttpPut("board-move")]
        public async Task<IActionResult> MoveCardToBoard([FromBody] MoveCardToBoardRequest request)
        {
            try
            {
                var card = await _cardService.GetByIdAsync(request.CardId);
                if (card == null)
                    return NotFound(new { message = "Card not found." });
                await _cardService.ClearCardLabelsAndMembers(card);
                var targetList = await _listService.GetByIdAsync(request.targetListId);
                if (targetList == null)
                    return NotFound(new { message = "Target list not found." });
                var targetListCards = await _cardService.GetCardsForListAsync(targetList);
                var listLastPosition = targetListCards.Any() ? targetListCards.Max(c => c.CardPosition) : 0;
                var sourceList = await _listService.GetByIdAsync(request.sourceListId);
                if (sourceList == null)
                    return NotFound(new { message = "Source list not found." });
                sourceList.Cards.Remove(card);
                card.CardPosition = listLastPosition + 1;
                targetList.Cards.Add(card);
                await _cardService.UpdateAsync(card);
                await _listService.UpdateAsync(sourceList);
                await _listService.UpdateAsync(targetList);

                return Ok($"list last position: ${listLastPosition + 1}");
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
