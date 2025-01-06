using TaskNetic.Client.DTO;
using TaskNetic.Data.Repository;
using TaskNetic.Models;

namespace TaskNetic.Services.Interfaces
{
    public interface ICardService : IRepository<Card>
    {
        Task<IEnumerable<Card>> GetCardsForListAsync(List list);
        Task AddCardToListAsync(List list, Card card);
        Task DeleteCardAsync(Card card);
        Task UpdateCardPositionsAsync(IEnumerable<CardPositionUpdate> updates);
        Task<Card?> GetFullCardInfoAsync(int cardId);
        Task<Card?> GetCardWithMembersAsync(int cardId);
        Task<List<ApplicationUser>> GetCardMembersAsync(int cardId);
        Task ClearCardLabelsAndMembers(Card card);
    }
}
