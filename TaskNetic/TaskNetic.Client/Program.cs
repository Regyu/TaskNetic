using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TaskNetic.Client;
using TaskNetic.Client.Services.Implementations;
using TaskNetic.Client.Services.Interfaces;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped(sp =>
{
    NavigationManager navigation = sp.GetRequiredService<NavigationManager>();
    return new HttpClient { BaseAddress = new Uri(navigation.BaseUri) };
});
builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrap5Components()
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISignalRService, SignalRService>();
builder.Services.AddScoped<IApplicationUserService, ApplicationUserService>();
await builder.Build().RunAsync();
