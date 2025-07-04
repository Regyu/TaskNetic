using TaskNeticDemo.Models;
using TaskNeticDemo.Services.Interfaces;


namespace TaskNeticDemo.Services.Implementations
{
    public class ApplicationUserService : IApplicationUserService
    {
        new List<ApplicationUser> users = new()
        {


        };

        public ApplicationUser GetByUserNameAsync(string userName)
        {

        }
        public ApplicationUser GetCurrentUserAsync()
        {
            return users[0];
        }
    }
}
