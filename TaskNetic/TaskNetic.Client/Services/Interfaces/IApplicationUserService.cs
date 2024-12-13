using TaskNetic.Client.DTO;
namespace TaskNetic.Client.Services.Interfaces
{
    public interface IApplicationUserService
    {
        Task<ApplicationUserInfo> GetByUserNameAsync(string userName);
    }
}
