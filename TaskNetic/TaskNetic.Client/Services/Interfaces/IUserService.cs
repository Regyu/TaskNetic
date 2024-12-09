﻿
namespace TaskNetic.Client.Services.Interfaces
{
    public interface IUserService
    {
        Task<string?> GetCurrentUserIdAsync();
        Task<bool> IsUserAdminInProjectAsync(int projectId, string userId);
    }
}
