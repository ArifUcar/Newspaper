using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Persistance.Context;

namespace UdemyCarBook.Persistance.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        private readonly NewsContext _context;

        public CommentRepository(NewsContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Comment>> GetAllWithDetailsAsync()
        {
            return await _context.Comments
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<Comment> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Comments
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<List<Comment>> GetActiveCommentsAsync()
        {
            return await _context.Comments
                .Where(x => !x.IsDeleted && x.IsApproved)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<Comment>> GetCommentsByNewsIdAsync(Guid newsId)
        {
            return await _context.Comments
                .Where(x => x.NewsId == newsId && !x.IsDeleted && x.IsApproved)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<Comment>> GetCommentsByUserIdAsync(Guid userId)
        {
            return await _context.Comments
                .Where(x => x.CreatedById == userId && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<Comment>> GetUnApprovedCommentsAsync()
        {
            return await _context.Comments
                .Where(x => !x.IsDeleted && !x.IsApproved)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<Comment>> GetApprovedCommentsAsync()
        {
            return await _context.Comments
                .Where(x => !x.IsDeleted && x.IsApproved)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<Comment>> GetRepliesByCommentIdAsync(Guid commentId)
        {
            return await _context.Comments
                .Where(x => x.ParentCommentId == commentId && !x.IsDeleted && x.IsApproved)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<int> GetCommentCountByNewsIdAsync(Guid newsId)
        {
            return await _context.Comments
                .CountAsync(x => x.NewsId == newsId && !x.IsDeleted && x.IsApproved);
        }
    }
}

