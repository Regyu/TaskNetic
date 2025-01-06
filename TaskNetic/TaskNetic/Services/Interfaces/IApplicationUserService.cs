using TaskNetic.Models;

namespace TaskNetic.Services.Interfaces
{
    public interface IApplicationUserService
    {
        Task<ApplicationUser> GetByUserNameAsync(string userName);
        Task<ApplicationUser?> GetUserByIdAsync(string userId);
        Task<ApplicationUser?> GetCurrentUserAsync();
        Task<string?> GetCurrentUserIdAsync();
        Task<bool> CheckIfUserExists(string userId);
    }
}
