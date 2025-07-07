using TaskNeticDemo.Models;
using TaskNeticDemo.Services.Interfaces;


namespace TaskNeticDemo.Services.Implementations
{
    public class ApplicationUserService : IApplicationUserService
    {
        List<ApplicationUser> users = new()
        {

            new ApplicationUser { Id = "1", UserName = "John", ImagePath=""},
            new ApplicationUser { Id = "2", UserName = "Mary", ImagePath=""},
            new ApplicationUser { Id = "3", UserName = "Peter", ImagePath=""},
            new ApplicationUser { Id = "4", UserName = "Jane", ImagePath=""},
            new ApplicationUser { Id = "5", UserName = "Mark", ImagePath=""}
        };

        public async Task<ApplicationUser> GetByUserNameAsync(string userName)
        {
            return users.FirstOrDefault(u => u.UserName == userName);
        }
        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return users[0];
        }

        public List<ApplicationUser> GetAllUsers()
        {
            return users;
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return users.FirstOrDefault(u => u.Id == id);
        }
    }
}
