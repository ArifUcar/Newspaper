using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Application.Interfaces.IService;
using UdemyCarBook.Persistance.Context;

namespace UdemyCarBook.Persistance.Service
{
    public class PermissionAuthorizationService : IPermissionAuthorizationService
    {
        private readonly NewsContext _context;
        private readonly IUserService _userService;

        public PermissionAuthorizationService(NewsContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<bool> HasPermissionAsync(Guid userId, string permissionCode)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);

            if (user == null)
                return false;

            return user.Roles.Any(r => r.IsActive && !r.IsDeleted &&
                r.Permissions.Any(p =>p.Code == permissionCode && p.IsActive && !p.IsDeleted));
        }

        public async Task<bool> HasAnyPermissionAsync(Guid userId, params string[] permissionCodes)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);

            if (user == null)
                return false;

            return user.Roles.Any(r => r.IsActive && !r.IsDeleted &&
                r.Permissions.Any(p => permissionCodes.Contains(p.Code) && p.IsActive && !p.IsDeleted));
        }

        public async Task<bool> HasAllPermissionsAsync(Guid userId, params string[] permissionCodes)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);

            if (user == null)
                return false;

            var userPermissions = user.Roles
                .Where(r => r.IsActive && !r.IsDeleted)
                .SelectMany(r => r.Permissions)
                .Where(p => p.IsActive && !p.IsDeleted)
                .Select(p => p.Code)
                .Distinct();

            return permissionCodes.All(code => userPermissions.Contains(code));
        }
    }
}