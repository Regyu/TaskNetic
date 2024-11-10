using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Implementations
{
    public interface ICommentService : IRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetCommentsByCardIdAsync(int cardId);
    }
}
