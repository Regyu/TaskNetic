using System.Net.Http.Json;
using TaskNetic.Client.DTO;
using TaskNetic.Client.Models;
using TaskNetic.Client.Services.Interfaces;

namespace TaskNetic.Client.Services.Implementations
{
    public class CardLabelsService : ICardLabelsService
    {
        public HttpClient _httpClient;
        private readonly IUserService _userService;
        public CardLabelsService(HttpClient httpClient, IUserService userService)
        {
            _httpClient = httpClient;
            _userService = userService;
        }

        public async Task<List<LabelModel>> GetBoardLabelsAsync(int boardId)
        {
            return (await _httpClient.GetFromJsonAsync<List<LabelModel>>($"api/labels/board/{boardId}")) ?? new();
        }

        public async Task<List<LabelModel>> GetCardLabelsAsync(int cardId)
        {
            return (await _httpClient.GetFromJsonAsync<List<LabelModel>>($"api/labels/card/{cardId}")) ?? new();
        }

        public async Task<bool> CreateBoardLabelAsync(int boardId, NewBoardLabel newLabel)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/labels/board/{boardId}", newLabel);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddLabelToCardAsync(int cardId, int labelId)
        {
            var request = new LabelCardRequest
            {
                CurrentUserId = _userService.GetCurrentUserId(),
                LabelId = labelId
            };
            var response = await _httpClient.PostAsJsonAsync($"api/labels/card/{cardId}", request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveLabelFromCardAsync(int cardId, int labelId)
        {
            var removeRequest = new LabelCardRequest
            {
                CurrentUserId = _userService.GetCurrentUserId(),
                LabelId = labelId
            };

            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/labels/card/{cardId}")
            {
                Content = JsonContent.Create(removeRequest)
            };

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteLabelFromBoardAsync(int labelId)
        {
            var response = await _httpClient.DeleteAsync($"api/labels/{labelId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateLabelAsync(LabelModel label)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/labels", label);
            return response.IsSuccessStatusCode;
        }
    }
}
