using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Implementations
{
    public interface ICardService : IRepository<Card>
    {
        Task<IEnumerable<Card>> GetCardsByListIdAsync(int listId);

    }
}
