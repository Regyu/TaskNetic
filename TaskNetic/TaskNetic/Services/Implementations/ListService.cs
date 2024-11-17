using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskNetic.Data;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Services.Implementations
{
    public class ListService : Repository<List>, IListService
    {

        public ListService(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<List>> GetListsForBoardAsync(Board board)
        {
            if (board == null)
            {
                throw new ArgumentNullException(nameof(board), "Board cannot be null.");
            }

            await _context.Entry(board).Collection(b => b.Lists).LoadAsync();

            // Filter boards for the current user
            //var lists = board.Lists
            //    .ToList();

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

            // Add the new board to the project
            board.Lists.Add(list);

            await _context.SaveChangesAsync();
        }


        public async Task DeleteListAsync(List list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list), "List cannot be null.");
            }

            // Ensure related BoardUsers are loaded
            _context.Entry(list).Collection(l => l.Cards).Load();

            // Clear the BoardUsers relation to avoid orphaned entries
            list.Cards.Clear();

            // Remove the board
            _context.Lists.Remove(list);

            await _context.SaveChangesAsync();
        }

    }
}
