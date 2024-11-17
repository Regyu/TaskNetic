using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Interfaces
{
    public interface ILabelService : IRepository<Label>
    {
        Task<IEnumerable<Label>> GetLabelsByCardAsync(Card card);
        Task AddLabelToCardAsync(Card card, Label label);
        Task DeleteLabelAsync(Label label);
    }
}