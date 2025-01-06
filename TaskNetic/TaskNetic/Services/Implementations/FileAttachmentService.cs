using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using TaskNetic.Data;
using TaskNetic.Data.Repository;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;

namespace TaskNetic.Services.Implementations
{
    public class FileAttachmentService : Repository<FileAttachment>, IFileAttachmentService
    {
        public FileAttachmentService(ApplicationDbContext context) : base(context)  {}

        public async Task<IEnumerable<FileAttachment>> GetAttachmentsByCardAsync(Card card)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card), "Comment cannot be null.");
            }

            await _context.Entry(card).Collection(c => c.Attachments).LoadAsync();

            return card.Attachments.AsEnumerable();
        }

        public async Task AddAttachmentToCardAsync(Card card, FileAttachment fileAttachment)
        {

            if (card == null)
            {
                throw new ArgumentNullException(nameof(card), "Card cannot be null.");
            }

            if (fileAttachment == null)
            {
                throw new ArgumentNullException(nameof(fileAttachment), "FileAttachment cannot be null.");
            }

            card.Attachments.Add(fileAttachment);

            await _context.SaveChangesAsync();
        }


        public async Task DeleteAttachmentAsync(FileAttachment fileAttachment)
        {
            if (fileAttachment == null)
            {
                throw new ArgumentNullException(nameof(fileAttachment), "FileAttachment cannot be null.");
            }

            _context.Attachments.Remove(fileAttachment);

            await _context.SaveChangesAsync();
        }
    }
}
