using TaskNetic.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace TaskNetic.Client.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly HttpClient _httpClient;
        public UserService(AuthenticationStateProvider authenticationStateProvider, HttpClient httpClient)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _httpClient = httpClient;
        }

        public async Task<string?> GetCurrentUserIdAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            return user.Identity?.IsAuthenticated == true
                ? user.FindFirst(u => u.Type.Contains("nameidentifier"))?.Value : null;
        }

        public string GetCurrentUserId()
        {
            var authState = _authenticationStateProvider.GetAuthenticationStateAsync().Result;
            var user = authState.User;

            return user.Identity?.IsAuthenticated == true
                ? user.FindFirst(u => u.Type.Contains("nameidentifier"))?.Value : null;
        }

        public async Task<bool> IsUserAdminInProjectAsync(int projectId, string userId)
        {
            var request = await _httpClient.GetAsync($"api/projectrole/is-admin/{projectId}/{userId}");
            var response = await request.Content.ReadAsStringAsync();
            return bool.Parse(response);
        }
    }
}
