
namespace TaskNetic.Client.Services.Interfaces
{
    public interface IUserService
    {
        Task<string?> GetCurrentUserIdAsync();
        string GetCurrentUserId();
        Task<bool> IsUserAdminInProjectAsync(int projectId, string userId);
        Task<bool> CanUserEditBoardAsync(int boardId, string userId);
    }
}
