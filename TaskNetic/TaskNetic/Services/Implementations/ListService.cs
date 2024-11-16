using Microsoft.EntityFrameworkCore;
using TaskNetic.Data;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Services.Implementations
{
    public class ListService : Repository<List>, IListService
    {
        //private readonly ApplicationDbContext _context;

        public ListService(ApplicationDbContext context) : base(context)
        {
            //_context = context;
        }

        public async Task<IEnumerable<List>> GetListsByBoardIdAsync(int boardId)
        {
            // Fetch lists belonging to a specific board
            return await _context.Set<Board>()
                .Where(board => board.BoardId == boardId)
                .SelectMany(board => board.Lists) // Extract the Lists collection from the Board
                .Include(list => list.Cards) // Eagerly load associated Cards
                .ToListAsync();
        }

        public async Task AddListByBoardIdAsync(int boardId, string listTitle)
        {
            // Find the board by ID
            var board = await _context.Set<Board>()
                .Include(b => b.Lists) // Ensure the Lists collection is loaded
                .FirstOrDefaultAsync(b => b.BoardId == boardId);

            if (board == null)
                throw new ArgumentException("Board not found.", nameof(boardId));

            // Create a new List and add it to the board's Lists collection
            var newList = new List
            {
                Title = listTitle,
                Cards = new List<Card>() // Initialize an empty card collection
            };

            board.Lists.Add(newList);

            // Save changes
            await _context.SaveChangesAsync();
        }

        public async Task DeleteListByBoardIdAsync(int boardId, int listId)
        {
            // Find the board with the specified ID and include its Lists
            var board = await _context.Set<Board>()
                .Include(b => b.Lists)
                .ThenInclude(list => list.Cards) // Include Cards for all Lists
                .FirstOrDefaultAsync(b => b.BoardId == boardId);

            if (board == null)
                throw new ArgumentException("Board not found.", nameof(boardId));

            // Find the specific list within the board
            var list = board.Lists.FirstOrDefault(l => l.Id == listId);

            if (list == null)
                throw new ArgumentException("List not found in the specified board.", nameof(listId));

            // Remove associated cards
            _context.Set<Card>().RemoveRange(list.Cards);

            // Remove the list from the board
            board.Lists.Remove(list);

            // Save changes
            await _context.SaveChangesAsync();
        }
    }
}
