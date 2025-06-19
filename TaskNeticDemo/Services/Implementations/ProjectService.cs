using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using TaskNetic.Client.DTO;
using TaskNetic.Client.Services.Interfaces;

namespace TaskNetic.Client.Services.Implementations
{
    public class ProjectService: IProjectService
    {
        private readonly HttpClient _httpClient;
        public ProjectService(HttpClient httpClient) {
            _httpClient = httpClient;
        }
        public async Task<ProjectInfo> GetProjectInfoAsync(int projectId)
        {
            var request = await _httpClient.GetFromJsonAsync<ProjectInfo>($"api/projects/{projectId}/info");
            return request;
        }
    }
}
