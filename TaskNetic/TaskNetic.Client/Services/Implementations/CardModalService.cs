using TaskNetic.Client.Services.Interfaces;

namespace TaskNetic.Client.Services.Implementations
{
    public class CardModalService : ICardModalService
    {
        private readonly HttpClient _httpClient;
        public CardModalService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


    }
}
