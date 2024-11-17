using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Interfaces
{
    public interface ICommentService : IRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetCommentsByCardAsync(Card card);
        Task AddCommentToCardAsync(Card card, Comment comment);
        Task DeleteCommentAsync(Comment comment);
    }
}
