using TaskNeticDemo.Models;

namespace TaskNeticDemo.Services.Interfaces
{
    public interface IApplicationUserService
    {
        Task<ApplicationUser> GetCurrentUserAsync();
        Task<ApplicationUser> GetByUserNameAsync(string userName);
        List<ApplicationUser> GetAllUsers();
        Task<ApplicationUser> GetUserByIdAsync(string id);
    }
}
