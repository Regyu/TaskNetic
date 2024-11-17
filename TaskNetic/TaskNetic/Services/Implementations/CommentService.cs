using TaskNetic.Data.Repository;
using TaskNetic.Data;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace TaskNetic.Services.Implementations
{
    public class CommentService : Repository<Comment>, ICommentService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public CommentService(ApplicationDbContext context, AuthenticationStateProvider authenticationStateProvider) : base(context)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByCardAsync(Card card)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card), "Comment cannot be null.");
            }

            await _context.Entry(card).Collection(c => c.Comments).LoadAsync();

            return card.Comments.AsEnumerable();
        }

        public async Task AddCommentToCardAsync(Card card, Comment comment)
        {

            if (card == null)
            {
                throw new ArgumentNullException(nameof(card), "Card cannot be null.");
            }

            if (comment == null)
            {
                throw new ArgumentNullException(nameof(comment), "List cannot be null.");
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

            comment.AuthorUserId = int.Parse(userId);

            card.Comments.Add(comment);

            await _context.SaveChangesAsync();
        }


        public async Task DeleteCommentAsync(Comment comment)
        {
            if (comment == null)
            {
                throw new ArgumentNullException(nameof(comment), "Comment cannot be null.");
            }

            _context.Comments.Remove(comment);

            await _context.SaveChangesAsync();
        }
    }
}
