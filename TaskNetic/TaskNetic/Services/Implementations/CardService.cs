using Microsoft.EntityFrameworkCore;
using TaskNetic.Data.Repository;
using TaskNetic.Data;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Services.Implementations
{
    public class CardService : Repository<Card>, ICardService
    {
        public CardService(ApplicationDbContext context) : base(context) { }

        public Task<IEnumerable<Card>> GetCardsByListIdAsync(int listId)
        {
            throw new NotImplementedException();
        }

        public async Task AddCardByListIdAsync(int listId)
        {  throw new NotImplementedException(); }

        public async Task DeleteCardByListIdAsync(int listId)
        { throw new NotImplementedException(); }
    }
}
