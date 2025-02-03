using Microsoft.EntityFrameworkCore;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Persistance.Context;

namespace UdemyCarBook.Persistance.Repositories
{
    public class SocialMediaRepository : Repository<SocialMedia>, ISocialMediaRepository
    {
        private readonly NewsContext _context;

        public SocialMediaRepository(NewsContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<SocialMedia>> GetAllWithDetailsAsync()
        {
            return await _context.SocialMedias
                .Include(x => x.User)
                .Include(x => x.CreatedByUser)
                .Include(x => x.LastModifiedByUser)
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();
        }

        public async Task<SocialMedia> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.SocialMedias
                .Include(x => x.User)
                .Include(x => x.CreatedByUser)
                .Include(x => x.LastModifiedByUser)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<List<SocialMedia>> GetByAuthorIdAsync(Guid authorId)
        {
            return await _context.SocialMedias
                .Include(x => x.User)
                .Where(x => x.UserId == authorId && !x.IsDeleted)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();
        }

        public async Task<List<SocialMedia>> GetActiveAccountsAsync()
        {
            return await _context.SocialMedias
                .Include(x => x.UserId)
                .Where(x => !x.IsDeleted && x.IsActive)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();
        }

        public async Task<bool> IsPlatformExistsForAuthorAsync(string platform, Guid? authorId)
        {
            return await _context.SocialMedias
                .AnyAsync(x => x.Platform == platform && 
                              x.UserId == authorId && 
                              !x.IsDeleted);
        }
    }
} 