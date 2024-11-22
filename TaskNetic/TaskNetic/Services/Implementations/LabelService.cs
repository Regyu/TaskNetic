using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using TaskNetic.Data;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Services.Implementations
{
    public class LabelService : Repository<Label>, ILabelService
    {
        public LabelService(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Label>> GetLabelsByCardAsync(Card card)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card), "Comment cannot be null.");
            }

            await _context.Entry(card).Collection(c => c.CardLabels).LoadAsync();

            return card.CardLabels.AsEnumerable();
        }

        public async Task AddLabelToCardAsync(Card card, Label label)
        {

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
