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
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public FileAttachmentService(ApplicationDbContext context, AuthenticationStateProvider authenticationStateProvider) : base(context)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

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
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated != true)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new InvalidOperationException("User ID is not available.");
            }

            fileAttachment.UploadedUserId = int.Parse(userId);

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
