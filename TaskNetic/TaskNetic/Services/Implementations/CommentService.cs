using TaskNetic.Data.Repository;
using TaskNetic.Data;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Services.Implementations
{
    public class CommentService : Repository<Comment>, ICommentService
    {
        public CommentService(ApplicationDbContext context) : base(context) { }

        public Task<IEnumerable<Comment>> GetCommentsByCardIdAsync(int cardId)
        {
            throw new NotImplementedException();
        }
    }
}
