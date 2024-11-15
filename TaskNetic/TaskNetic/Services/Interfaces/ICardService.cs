using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Interfaces
{
    public interface ICardService : IRepository<Card>
    {
        Task<IEnumerable<Card>> GetCardsByListIdAsync(int listId);

    }
}
