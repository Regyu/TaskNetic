using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskNetic.Client.Pages;
using TaskNetic.Data;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;
using Board = TaskNetic.Models.Board;

namespace TaskNetic.Services.Implementations
{
    public class ListService : Repository<List>, IListService
    {

        public ListService(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<List>> GetListsForBoardAsync(Board board)
        {
            var baseBoard = await _context.Boards
            .Include(b => b.Lists)
                .ThenInclude(l => l.Cards)
            .FirstOrDefaultAsync(b => b.BoardId == board.BoardId);

            if (board == null)
            {
                throw new ArgumentException($"Board with ID {board.BoardId} not found.", nameof(board.BoardId));
            }

            return board.Lists.AsEnumerable();
        }

        public async Task AddListToBoardsAsync(Board board, List list)
        {

            if (board == null)
            {
                throw new ArgumentNullException(nameof(board), "Board cannot be null.");
            }

            if (list == null)
            {
                throw new ArgumentNullException(nameof(list), "List cannot be null.");
            }

            board.Lists.Add(list);

            await _context.SaveChangesAsync();
        }


        public async Task DeleteListAsync(List list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list), "List cannot be null.");
            }

            _context.Entry(list).Collection(l => l.Cards).Load();

            _context.Cards.RemoveRange(list.Cards);

            _context.Lists.Remove(list);

            await _context.SaveChangesAsync();
        }

        public async Task MoveCardAsync(int cardId, int sourceListId, int targetListId, int newPosition)
        {
            // Find the source and target lists
            var sourceList = await _context.Lists.Include(l => l.Cards).FirstOrDefaultAsync(l => l.Id == sourceListId);
            var targetList = await _context.Lists.Include(l => l.Cards).FirstOrDefaultAsync(l => l.Id == targetListId);

            if (sourceList == null || targetList == null)
                throw new ArgumentException("Source or target list not found.");

            // Find the card in the source list
            var card = sourceList.Cards.FirstOrDefault(c => c.CardId == cardId);
            if (card == null)
                throw new ArgumentException($"Card with ID {cardId} not found in source list.");

            // Remove card from source list
            sourceList.Cards.Remove(card);

            // Adjust positions in the source list
            int position = 1;
            foreach (var c in sourceList.Cards.OrderBy(c => c.CardPosition))
            {
                c.CardPosition = position++;
            }

            // Add card to target list at the specified position
            foreach (var c in targetList.Cards.Where(c => c.CardPosition >= newPosition))
            {
                c.CardPosition++;
            }
            card.CardPosition = newPosition;
            targetList.Cards.Add(card);

            // Save changes to the database
            await _context.SaveChangesAsync();
        }

    }
}
