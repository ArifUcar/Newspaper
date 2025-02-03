using Microsoft.EntityFrameworkCore;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Persistance.Context;

namespace UdemyCarBook.Persistance.Repositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        private readonly NewsContext _context;

        public TagRepository(NewsContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Tag>> GetAllWithDetailsAsync()
        {
            return await _context.Tags
                .Include(x => x.News)
                .Include(x => x.CreatedByUser)
                .Include(x => x.LastModifiedByUser)
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Tag> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Tags
                .Include(x => x.News)
                .Include(x => x.CreatedByUser)
                .Include(x => x.LastModifiedByUser)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<List<Tag>> GetActiveTagsAsync()
        {
            return await _context.Tags
                .Include(x => x.News)
                .Where(x => !x.IsDeleted && x.News.Any(n => !n.IsDeleted))
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<bool> IsNameExistsAsync(string name)
        {
            return await _context.Tags
                .AnyAsync(x => x.Name == name && !x.IsDeleted);
        }

        public async Task<List<Tag>> GetTagsByNewsCountAsync(int minNewsCount)
        {
            return await _context.Tags
                .Include(x => x.News)
                .Where(x => !x.IsDeleted && x.News.Count(n => !n.IsDeleted) >= minNewsCount)
                .OrderByDescending(x => x.News.Count(n => !n.IsDeleted))
                .ToListAsync();
        }

        public async Task<List<Tag>> GetTagsByNewsIdAsync(Guid newsId)
        {
            return await _context.Tags
                .Include(x => x.News)
                .Where(x => !x.IsDeleted && x.News.Any(n => n.Id == newsId && !n.IsDeleted))
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
    }
} 