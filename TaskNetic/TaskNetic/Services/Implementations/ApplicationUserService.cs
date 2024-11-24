using TaskNetic.Data;
using TaskNetic.Models;
using TaskNetic.Data.Repository;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Services.Implementations
{
    public class ApplicationUserService : Repository<ApplicationUser>, IApplicationUserService
    {
        public ApplicationUserService(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<ApplicationUser> GetByUserNameAsync(string userName)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == userName);
        }
    }
}
