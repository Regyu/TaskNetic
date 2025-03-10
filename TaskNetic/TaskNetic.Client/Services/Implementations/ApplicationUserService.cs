﻿using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using TaskNetic.Client.DTO;
using TaskNetic.Client.Services.Interfaces;


namespace TaskNetic.Client.Services.Implementations
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly HttpClient _httpClient;

        public ApplicationUserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetIdByUserNameAsync(string userName)
        {
            var request = await _httpClient.GetFromJsonAsync<ApplicationUserInfo>($"api/applicationusers/get-user-id/{userName}");
            return request.Id;
        }
    }
}
