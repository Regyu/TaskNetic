using TaskNetic.Client.Models;

namespace TaskNetic.Client.Services.Interfaces
{
    public interface ICardModalService
    {
        Task<bool> AddMemberToCardAsync(int cardId, string userId);
        Task<bool> RemoveMemberFromCardAsync(int? cardId, string userId);
        Task<List<ApplicationUser>> GetCardMembers(int cardId);
        Task<bool> CreateTodoTaskAsync(int cardId, string taskName);
        Task<List<TodoTask>> GetTodoTasksByCardAsync(int cardId);
        Task UpdateTodoTaskAsync(TodoTask todoTask);
        Task DeleteTodoTaskAsync(int todoTaskId);
        Task<List<Comment>> GetCommentsAsync(int cardId);
        Task<bool> AddCommentToCardAsync(int cardId, string comment);
        Task<bool> DeleteCommentAsync(int commentId);
        Task<bool> UpdateCommentAsync(int commentId, string comment);
        Task<bool> RemoveCardAsync(int cardId);
    }
}
