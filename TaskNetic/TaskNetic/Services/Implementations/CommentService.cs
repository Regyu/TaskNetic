﻿using TaskNetic.Data.Repository;
using TaskNetic.Data;
using TaskNetic.Models;
using TaskNetic.Services.Interfaces;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Security;
using TaskNetic.Client.Pages;

namespace TaskNetic.Services.Implementations
{
    public class CommentService : Repository<Comment>, ICommentService
    {
        private readonly ApplicationUserService _applicationUserService;
        public CommentService(ApplicationDbContext context, AuthenticationStateProvider authenticationStateProvider) : base(context)
        {
            _applicationUserService = new ApplicationUserService(context, authenticationStateProvider);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByCardAsync(Card card)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card), "Comment cannot be null.");
            }

            await _context.Entry(card).Collection(c => c.Comments).Query().Include(comment => comment.User).LoadAsync();

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
