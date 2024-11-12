using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Interfaces
{
    public interface ICommentService : IRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetCommentsByCardIdAsync(int cardId);
    }
}
