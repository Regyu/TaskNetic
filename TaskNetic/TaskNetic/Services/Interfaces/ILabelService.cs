using TaskNetic.Client.DTO;
using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Interfaces
{
    public interface ILabelService : IRepository<Label>
    {
        Task<List<Label>> GetLabelsByCardAsync(int cardId);
        Task AddLabelToCardAsync(int cardId, Label label);
        Task DeleteLabelAsync(Label label);
        Task<List<Label>> GetLabelsByBoardAsync(int boardId);
        Task AddBoardLabel(int BoardId, NewBoardLabel label);
        Task RemoveLabelFromCardAsync(int cardId, int labelId);
    }
}