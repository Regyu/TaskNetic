using TaskNetic.Models;

namespace TaskNetic.Services.Interfaces
{
    public interface IApplicationUserService
    {
        Task<ApplicationUser> GetByUserNameAsync(string userName);
        Task<string?> GetCurrentUserIdAsync();
    }
}
