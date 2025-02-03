using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Persistance.Context;

namespace UdemyCarBook.Persistance.Repositories
{
    public class NewsRepository : Repository<News>, INewsRepository
    {
        private readonly NewsContext _context;

        public NewsRepository(NewsContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<News>> GetAllWithDetailsAsync()
        {
            return await _context.News
                .Include(x => x.Category)
                .Include(x => x.User)
                .Include(x => x.CreatedByUser)
                .Include(x => x.LastModifiedByUser)
                .Include(x => x.Tags)
                .Include(x => x.Comments)
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<News> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.News
                .Include(x => x.Category)
                .Include(x => x.User)
                .Include(x => x.CreatedByUser)
                .Include(x => x.LastModifiedByUser)
                .Include(x => x.Tags)
                .Include(x => x.Comments)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<List<News>> GetNewsByCategoryAsync(Guid categoryId)
        {
            return await _context.News
                .Include(x => x.Category)
                .Include(x => x.User)
                .Include(x => x.Tags)
                .Where(x => x.CategoryId == categoryId && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<News>> GetNewsByUserAsync(Guid userId)
        {
            return await _context.News
                .Include(x => x.Category)
                .Include(x => x.User)
                .Include(x => x.Tags)
                .Where(x => x.UserId == userId && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<News>> GetNewsByTagAsync(Guid tagId)
        {
            return await _context.News
                .Include(x => x.Category)
                .Include(x => x.User)
                .Include(x => x.Tags)
                .Where(x => x.Tags.Any(t => t.Id == tagId) && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<News>> SearchNewsAsync(string keyword)
        {
            return await _context.News
                .Include(x => x.Category)
                .Include(x => x.User)
                .Include(x => x.Tags)
                .Where(x => !x.IsDeleted && 
                    (x.Title.Contains(keyword) || 
                    x.Content.Contains(keyword) || 
                    x.Summary.Contains(keyword)))
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<News>> GetLatestNewsAsync(int count)
        {
            return await _context.News
                .Include(x => x.Category)
                .Include(x => x.User)
                .Include(x => x.Tags)
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<News>> GetPopularNewsAsync(int count)
        {
            return await _context.News
                .Include(x => x.Category)
                .Include(x => x.User)
                .Include(x => x.Tags)
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.ViewCount)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<News>> GetFeaturedNewsAsync()
        {
            return await _context.News
                .Include(x => x.Category)
                .Include(x => x.User)
                .Include(x => x.Tags)
                .Where(x => x.IsFeatured && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<bool> IsNewsExistsAsync(Guid id)
        {
            return await _context.News.AnyAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<bool> IsNewsSlugExistsAsync(string slug)
        {
            return await _context.News.AnyAsync(x => x.Slug == slug && !x.IsDeleted);
        }

        public async Task IncrementViewCountAsync(Guid id)
        {
            var news = await _context.News.FindAsync(id);
            if (news != null)
            {
                news.ViewCount++;
                await _context.SaveChangesAsync();
            }
        }
    }
} 