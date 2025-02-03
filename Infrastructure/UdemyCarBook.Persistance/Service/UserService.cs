using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UdemyCarBook.Application.Interfaces.IService;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Persistance.Context;

namespace UdemyCarBook.Persistance.Service
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly NewsContext _context;

        public UserService(IHttpContextAccessor httpContextAccessor, NewsContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<User> GetCurrentUserAsync()
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId == Guid.Empty)
                return null;

            return await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);
        }

        public Task<Guid> GetCurrentUserIdAsync()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                return Task.FromResult(Guid.Empty);

            return Task.FromResult(userId);
        }

        public async Task<string> GetUserNameAsync(Guid userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);
            return user?.UserName;
        }

        public async Task<bool> IsInRoleAsync(Guid userId, string roleName)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);

            if (user == null)
                return false;

            return user.Roles.Any(r => r.Name.ToUpper() == roleName.ToUpper() && r.IsActive && !r.IsDeleted);
        }
    }
}