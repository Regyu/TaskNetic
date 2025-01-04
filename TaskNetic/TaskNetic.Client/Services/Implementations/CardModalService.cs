using System.Net.Http.Json;
using TaskNetic.Client.DTO;
using TaskNetic.Client.Models;
using TaskNetic.Client.Pages;
using TaskNetic.Client.Services.Interfaces;
using static System.Net.WebRequestMethods;

namespace TaskNetic.Client.Services.Implementations
{
    public class CardModalService : ICardModalService
    {
        private readonly HttpClient _httpClient;
        private readonly IUserService UserService;
        public CardModalService(HttpClient httpClient, IUserService userService)
        {
            _httpClient = httpClient;
            UserService = userService;
        }

        public async Task<bool> RemoveCardAsync(int cardId)
        {
            var currentUserId = await UserService.GetCurrentUserIdAsync();
            var response = await _httpClient.DeleteAsync($"api/cards/{cardId}/{currentUserId}");
            return response.IsSuccessStatusCode;
        }

            public async Task<bool> AddMemberToCardAsync(int cardId, string userId)
        {
            var newMember = new NewUserString
            {
                Text = userId,
                CurrentUserId = UserService.GetCurrentUserId()
            };
            var response = await _httpClient.PostAsJsonAsync($"api/cards/{cardId}/members/", newMember);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveMemberFromCardAsync(int? cardId, string userId)
        {
            if(cardId == null)
                return false;

            var currentUserId = UserService.GetCurrentUserId();

            var removeMember = new NewUserString
            {
                Text = userId,
                CurrentUserId = currentUserId
            };

            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/cards/{cardId}/members")
            {
                Content = JsonContent.Create(removeMember)
            };

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<ApplicationUser>> GetCardMembers(int cardId)
        {
            return await _httpClient.GetFromJsonAsync<List<ApplicationUser>>($"api/cards/{cardId}/members") ?? new List<ApplicationUser>();
        }

        public async Task<bool> CreateTodoTaskAsync(int cardId, string taskName)
        {
            var currentUserId = UserService.GetCurrentUserId();
            var todoTask = new NewUserString
            {
                CurrentUserId = currentUserId,
                Text = taskName
            };

            var response = await _httpClient.PostAsJsonAsync($"api/todotasks/card/{cardId}", todoTask);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<TodoTask>> GetTodoTasksByCardAsync(int cardId)
        {
            return await _httpClient.GetFromJsonAsync<List<TodoTask>>($"api/todotasks/card/{cardId}") ?? new List<TodoTask>();
        }

        public async Task UpdateTodoTaskAsync(TodoTask todoTask)
        {
            await _httpClient.PutAsJsonAsync($"api/todotasks", todoTask);
        }

        public async Task DeleteTodoTaskAsync(int todoTaskId)
        {
            await _httpClient.DeleteAsync($"api/todotasks/{todoTaskId}");
        }

        public async Task<List<Comment>> GetCommentsAsync(int cardId)
        {
            return await _httpClient.GetFromJsonAsync<List<Comment>>($"api/comments/card/{cardId}") ?? new List<Comment>();
        }

        public async Task<bool> AddCommentToCardAsync(int cardId, string comment)
        {
            var currentUserId = await UserService.GetCurrentUserIdAsync();
            if (currentUserId == null)
                return false;
            var currentTime = DateTime.UtcNow;
            var newComment = new NewCommentRequest
            {
                Comment = comment,
                userId = currentUserId,
                creationDate = currentTime
            };
            var response = await _httpClient.PostAsJsonAsync($"api/comments/card/{cardId}", newComment);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCommentAsync(int commentId)
        {
            var response = await _httpClient.DeleteAsync($"api/comments/{commentId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCommentAsync(int commentId, string comment)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/comments/{commentId}", comment);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCardDueDateAsync(int cardId, DateTime? date)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/cards/{cardId}/due-date", date);
            return response.IsSuccessStatusCode;
        }

    }
}
