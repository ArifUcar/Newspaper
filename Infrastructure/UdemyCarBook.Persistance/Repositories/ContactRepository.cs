using Microsoft.EntityFrameworkCore;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Persistance.Context;

namespace UdemyCarBook.Persistance.Repositories
{
    public class ContactRepository : Repository<Contact>, IContactRepository
    {
        private readonly NewsContext _context;

        public ContactRepository(NewsContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Contact>> GetAllWithDetailsAsync()
        {
            return await _context.Contacts
                .Include(x => x.CreatedByUser)
                .Include(x => x.LastModifiedByUser)
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<Contact> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Contacts
                .Include(x => x.CreatedByUser)
                .Include(x => x.LastModifiedByUser)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<List<Contact>> GetUnreadContactsAsync()
        {
            return await _context.Contacts
                .Include(x => x.CreatedByUser)
                .Where(x => !x.IsDeleted && !x.IsRead)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<Contact>> GetUnansweredContactsAsync()
        {
            return await _context.Contacts
                .Include(x => x.CreatedByUser)
                .Where(x => !x.IsDeleted && !x.IsReplied)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }
    }
} 