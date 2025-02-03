using Microsoft.EntityFrameworkCore;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Persistance.Context;

namespace UdemyCarBook.Persistance.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly NewsContext _context;

        public CategoryRepository(NewsContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllWithDetailsAsync()
        {
            return await _context.Categories
                .Include(c => c.News)
                .Include(c => c.CreatedByUser)
                .Include(c => c.UpdatedByUser)
                .Include(c => c.LastModifiedByUser)
                .Where(c => !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<Category> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Categories
                .Include(c => c.News)
                .Include(c => c.CreatedByUser)
                .Include(c => c.UpdatedByUser)
                .Include(c => c.LastModifiedByUser)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }

        public async Task<bool> IsCategoryExistsAsync(string name)
        {
            return await _context.Categories
                .AnyAsync(c => c.Name.ToLower() == name.ToLower() && !c.IsDeleted);
        }
    }
} 