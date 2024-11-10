using Microsoft.EntityFrameworkCore;
using TaskNetic.Data.Repository;
using TaskNetic.Data;
using TaskNetic.Models;
using TaskNetic.Services.Implementations;

namespace TaskNetic.Services.Interfaces
{
    public class CardService : Repository<Card>, ICardService
    {
        public CardService(ApplicationDbContext context) : base(context) { }

        public Task<IEnumerable<Card>> GetCardsByListIdAsync(int listId)
        {
            throw new NotImplementedException();
        }
    }
}
