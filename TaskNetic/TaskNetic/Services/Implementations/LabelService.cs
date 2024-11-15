using TaskNetic.Data;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Services.Implementations
{
    public class LabelService : Repository<Label>, ILabelService
    {
        public LabelService(ApplicationDbContext context) : base(context) { }

        public Task<IEnumerable<Label>> GetLabelsByBoardIdAsync(int boardId)
        {
            throw new NotImplementedException();
        }
    }
}
