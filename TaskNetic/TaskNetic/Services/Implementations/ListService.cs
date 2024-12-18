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

            list.Cards.Clear();

            _context.Lists.Remove(list);

            await _context.SaveChangesAsync();
        }

    }
}
