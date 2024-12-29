using System.Reflection.Emit;

namespace TaskNetic.Client.Services.Interfaces
{
    public interface ICardLabelsService
    {
        Task<string> GetBoardLabelsAsync(int cardId);
        Task<bool> AddLabelToCardAsync(int cardId, Label newLabel);
        Task<bool> RemoveLabelFromCardAsync(int cardId, int labelId);
    }
}
