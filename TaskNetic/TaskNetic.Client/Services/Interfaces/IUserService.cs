
namespace TaskNetic.Client.Services.Interfaces
{
    public interface IUserService
    {
        Task<string?> GetCurrentUserIdAsync();
    }
}
