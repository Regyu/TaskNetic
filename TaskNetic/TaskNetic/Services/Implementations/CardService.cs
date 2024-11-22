using Microsoft.EntityFrameworkCore;
using TaskNetic.Data.Repository;
using TaskNetic.Data;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;

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
        _context.Entry(card).Collection(c => c.NotificationUsers).Load();

        card.Comments.Clear();
        card.Attachments.Clear();
        card.CardLabels.Clear();
        card.CardMembers.Clear();
        card.NotificationUsers.Clear();

        _context.Cards.Remove(card);

        await _context.SaveChangesAsync();
    }

}
}
