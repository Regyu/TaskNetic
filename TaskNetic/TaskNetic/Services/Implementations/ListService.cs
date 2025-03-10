﻿using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskNetic.Client.DTO;
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
                .ThenInclude(card => card.CardLabels)
                .Include(b => b.Lists)
                .ThenInclude(l => l.Cards)
                .ThenInclude(card => card.CardMembers)
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

            foreach (var card in list.Cards)
            {
                _context.Entry(card).Collection(c => c.Comments).Load();
                _context.Entry(card).Collection(c => c.Attachments).Load();
                _context.Entry(card).Collection(c => c.TodoTasks).Load();
                _context.Entry(card).Collection(c => c.CardLabels).Load();
                _context.Entry(card).Collection(c => c.CardMembers).Load();

                _context.Comments.RemoveRange(card.Comments);
                _context.Attachments.RemoveRange(card.Attachments);
                _context.TodoTasks.RemoveRange(card.TodoTasks);

                card.CardLabels.Clear();
                card.CardMembers.Clear();

                _context.Cards.Remove(card);
            }

            _context.Cards.RemoveRange(list.Cards);

            _context.Lists.Remove(list);

            await _context.SaveChangesAsync();
        }

        public async Task MoveCardAsync(int cardId, int sourceListId, int targetListId, int newPosition)
        {
            var sourceList = await _context.Lists.Include(l => l.Cards).FirstOrDefaultAsync(l => l.Id == sourceListId);
            var targetList = await _context.Lists.Include(l => l.Cards).FirstOrDefaultAsync(l => l.Id == targetListId);

            if (sourceList == null || targetList == null)
                throw new ArgumentException("Source or target list not found.");

            var card = sourceList.Cards.FirstOrDefault(c => c.CardId == cardId);
            if (card == null)
                throw new ArgumentException($"Card with ID {cardId} not found in source list.");

            sourceList.Cards.Remove(card);

            int position = 1;
            foreach (var c in sourceList.Cards.OrderBy(c => c.CardPosition))
            {
                c.CardPosition = position++;
            }

            foreach (var c in targetList.Cards.Where(c => c.CardPosition >= newPosition))
            {
                c.CardPosition++;
            }
            card.CardPosition = newPosition;
            targetList.Cards.Add(card);

            await _context.SaveChangesAsync();
        }

        public async Task MoveListsAsync(IEnumerable<MoveListsRequest> listUpdates)
        {
            foreach (var update in listUpdates)
            {
                var list = await _context.Lists.FindAsync(update.ListId);
                if (list != null)
                {
                    list.Position = update.Position;
                }
            }

            await _context.SaveChangesAsync();
        }

    }
}
