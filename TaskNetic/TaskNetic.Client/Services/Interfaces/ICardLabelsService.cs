using TaskNetic.Client.DTO;
using TaskNetic.Client.Models;

namespace TaskNetic.Client.Services.Interfaces
{
    public interface ICardLabelsService
    {
        Task<List<LabelModel>> GetBoardLabelsAsync(int boardId);
        Task<List<LabelModel>> GetCardLabelsAsync(int cardId);
        Task<bool> CreateBoardLabelAsync(int boardId, NewBoardLabel newLabel);
        Task<bool> AddLabelToCardAsync(int cardId, int labelId);
        Task<bool> RemoveLabelFromCardAsync(int cardId, int labelId);
        Task<bool> DeleteLabelFromBoardAsync(int labelId);
        Task<bool> UpdateLabelAsync(LabelModel label);
    }
}
