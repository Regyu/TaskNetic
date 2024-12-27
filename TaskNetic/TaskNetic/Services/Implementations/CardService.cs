using Microsoft.EntityFrameworkCore;
using TaskNetic.Data.Repository;
using TaskNetic.Data;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using TaskNetic.Client.DTO;

namespace TaskNetic.Services.Implementations
{
    public class CardService : Repository<Card>, ICardService
    {

        public CardService(ApplicationDbContext context) : base(context) { }
        public async Task<IEnumerable<Card>> GetCardsForListAsync(List list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list), "List cannot be null.");
            }

            await _context.Entry(list).Collection(l => l.Cards).LoadAsync();

            //var cards = list.Cards
            //    .ToList();

            return list.Cards.AsEnumerable();
        }
        public async Task<Card?> GetFullCardInfoAsync(int cardId)
        {
            return await _context.Cards
                .Include(c => c.Comments) // Include comments
                .Include(c => c.Attachments) // Include attachments
                .Include(c => c.TodoTasks) // Include task list
                .Include(c => c.CardLabels) // Include labels
                .Include(c => c.CardMembers) // Include members
                .FirstOrDefaultAsync(c => c.CardId == cardId);
        }

        public async Task AddCardToListAsync(List list, Card card)
        {

            if (list == null)
            {
                throw new ArgumentNullException(nameof(list), "List cannot be null.");
            }

            if (card == null)
            {
                throw new ArgumentNullException(nameof(card), "Card cannot be null.");
            }

            list.Cards.Add(card);

            await _context.SaveChangesAsync();
        }


        public async Task DeleteCardAsync(Card card)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card), "Card cannot be null.");
            }

            _context.Entry(card).Collection(c => c.Comments).Load();
            _context.Entry(card).Collection(c => c.Attachments).Load();
            _context.Entry(card).Collection(c => c.CardLabels).Load();
            _context.Entry(card).Collection(c => c.CardMembers).Load();

            _context.Comments.RemoveRange(card.Comments);
            _context.Attachments.RemoveRange(card.Attachments);

            card.CardLabels.Clear();
            card.CardMembers.Clear();

            _context.Cards.Remove(card);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateCardPositionsAsync(IEnumerable<CardPositionUpdate> updates)
        {
            foreach (var update in updates)
            {
                var card = await _context.Cards.FindAsync(update.CardId);
                if (card != null)
                {
                    card.CardPosition = update.CardPosition;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
