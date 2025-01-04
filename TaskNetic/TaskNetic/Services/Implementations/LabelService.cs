using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskNetic.Client.DTO;
using TaskNetic.Data;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Services.Implementations
{
    public class LabelService : Repository<Label>, ILabelService
    {
        public LabelService(ApplicationDbContext context) : base(context) { }

        public async Task<List<Label>> GetLabelsByCardAsync(int cardId)
        {
            var card = await _context.Cards
                .Include(c => c.CardLabels)
                .FirstOrDefaultAsync(c => c.CardId == cardId);
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card), "Comment cannot be null.");
            }

            await _context.Entry(card).Collection(c => c.CardLabels).LoadAsync();

            return card.CardLabels.ToList();
        }

        public async Task<List<Label>> GetLabelsByBoardAsync(int boardId)
        {
            var board = await _context.Boards
                .Include(b => b.Labels)
                .FirstOrDefaultAsync(b => b.BoardId == boardId);

            if (board == null)
            {
                throw new ArgumentException($"Board with ID {boardId} not found.");
            }

            return board.Labels.ToList();
        }

        public async Task AddBoardLabel(int BoardId, NewBoardLabel label)
        {
            var board = await _context.Boards
            .Include(b => b.Labels)
        .   FirstOrDefaultAsync(b => b.BoardId == BoardId);

            if (board == null)
            {
                throw new ArgumentException($"Board with ID {BoardId} not found.");
            }

            var newLabel = new Label
            {
                LabelName = label.LabelName,
                ColorCode = label.ColorCode,
                Comment = label.Comment
            };
            await _context.Labels.AddAsync(newLabel);
            board.Labels.Add(newLabel);

            await _context.SaveChangesAsync();
        }

        public async Task AddLabelToCardAsync(int CardId, Label label)
        {
            var card = _context.Cards
                .Include(c => c.CardLabels)
                .FirstOrDefault(c => c.CardId == CardId);
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card), "Card cannot be null.");
            }

            if (label == null)
            {
                throw new ArgumentNullException(nameof(label), "Label cannot be null.");
            }


            card.CardLabels.Add(label);

            await _context.SaveChangesAsync();
        }

        public async Task RemoveLabelFromCardAsync(int cardId, int labelId)
        {
            var card = await _context.Cards
                .Include(c => c.CardLabels)
                .FirstOrDefaultAsync(c => c.CardId == cardId);
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card), "Card cannot be null.");
            }

            var label = card.CardLabels.FirstOrDefault(l => l.Id == labelId);
            if (label == null)
            {
                throw new ArgumentNullException(nameof(label), "Label cannot be null.");
            }

            card.CardLabels.Remove(label);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteLabelAsync(Label label)
        {
            if (label == null)
            {
                throw new ArgumentNullException(nameof(label), "Label cannot be null.");
            }

            _context.Labels.Remove(label);

            await _context.SaveChangesAsync();
        }

    }
 }
