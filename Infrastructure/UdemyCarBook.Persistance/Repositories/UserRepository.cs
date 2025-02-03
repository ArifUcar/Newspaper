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
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly NewsContext _context;

        public UserRepository(NewsContext context) : base(context)
        {
            _context = context;
        }

        public DbContext Context => _context;

        public async Task<List<User>> GetAllWithDetailsAsync()
        {
            var query = _context.Users
                .Include(x => x.Roles)
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.UserName);

            return await query.ToListAsync();
        }

        public async Task<User> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Users
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<List<User>> GetActiveUsersAsync()
        {
            return await _context.Users
                .Include(x => x.Roles)
                .Where(x => !x.IsDeleted && x.IsActive)
                .OrderBy(x => x.UserName)
                .ToListAsync();
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _context.Users
                .AnyAsync(x => x.Email == email && !x.IsDeleted);
        }

        public async Task<bool> IsUsernameExistsAsync(string username)
        {
            return await _context.Users
                .AnyAsync(x => x.UserName == username && !x.IsDeleted);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Email == email && !x.IsDeleted);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.UserName == username && !x.IsDeleted);
        }

        public async Task<List<User>> GetUsersByRoleAsync(string roleName)
        {
            return await _context.Users
                .Include(x => x.Roles)
                .Where(x => !x.IsDeleted && x.Roles.Any(r => r.Name == roleName && !r.IsDeleted))
                .OrderBy(x => x.UserName)
                .ToListAsync();
        }

        public async Task<bool> IsInRoleAsync(Guid userId, string roleName)
        {
            return await _context.Users
                .AnyAsync(x => x.Id == userId && 
                              !x.IsDeleted && 
                              x.Roles.Any(r => r.Name == roleName && !r.IsDeleted));
        }

        public override async Task CreateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Kullanıcı oluşturulurken bir hata oluştu: {ex.Message}", 
                    ex
                );
            }
        }
    }
} 