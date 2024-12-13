using Microsoft.AspNetCore.Components;
using TaskNetic.Client.Services.Implementations;
using TaskNetic.Client.Services.Interfaces;

namespace TaskNetic.Client
{
    public class ClientRegistry
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped(sp =>
            {
                NavigationManager navigation = sp.GetRequiredService<NavigationManager>();
                return new HttpClient { BaseAddress = new Uri(navigation.BaseUri) };
            });
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISignalRService, SignalRService>();
            services.AddScoped<IApplicationUserService, ApplicationUserService>();
        }
    }
}
