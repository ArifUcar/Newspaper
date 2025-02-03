using Microsoft.EntityFrameworkCore;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Persistance.Context;

namespace UdemyCarBook.Persistance.Repositories
{
    public class NewsletterRepository : Repository<Newsletter>, INewsletterRepository
    {
        private readonly NewsContext _context;

        public NewsletterRepository(NewsContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Newsletter>> GetAllWithDetailsAsync()
        {
            return await _context.Newsletters
                .Include(x => x.CreatedByUser)
                .Include(x => x.LastModifiedByUser)
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<Newsletter> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Newsletters
                .Include(x => x.CreatedByUser)
                .Include(x => x.LastModifiedByUser)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<List<Newsletter>> GetActiveSubscribersAsync()
        {
            return await _context.Newsletters
                .Where(x => !x.IsDeleted && x.IsActive && x.IsVerified)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _context.Newsletters
                .AnyAsync(x => x.Email == email && !x.IsDeleted);
        }

        public async Task<bool> IsEmailSubscribedAsync(string email)
        {
            return await _context.Newsletters
                .AnyAsync(x => x.Email == email && !x.IsDeleted && x.IsActive && x.IsVerified && !x.UnsubscribeDate.HasValue);
        }
    }
} 