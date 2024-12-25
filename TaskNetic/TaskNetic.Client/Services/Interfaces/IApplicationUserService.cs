using TaskNetic.Client.DTO;
namespace TaskNetic.Client.Services.Interfaces
{
    public interface IApplicationUserService
    {
        Task<string> GetIdByUserNameAsync(string userName);
    }
}
