using TaskNetic.Client.Services.Interfaces;
using TaskNetic.Client.Services.Implementations;

namespace TaskNetic.Client
{
    public class ClientRegistry
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
        }
    }
}
