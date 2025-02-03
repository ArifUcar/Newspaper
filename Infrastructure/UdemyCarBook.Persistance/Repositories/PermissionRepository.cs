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
    public class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        private readonly NewsContext _context;

        public PermissionRepository(NewsContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Permission>> GetAllWithDetailsAsync()
        {
            var permissions = await _context.Permissions
                .Include(x => x.Roles)
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Group)
                .ThenBy(x => x.Name)
                .ToListAsync();

            return permissions.Select(permission => new Permission
            {
                Id = permission.Id,
                Name = permission.Name,
                Description = permission.Description,
                Group = permission.Group,
                Code = permission.Code,
                IsActive = permission.IsActive,
                IsDeleted = permission.IsDeleted,
                CreatedById = permission.CreatedById,
                CreatedDate = permission.CreatedDate,
                UpdatedByUserId = permission.UpdatedByUserId,
                LastModifiedByUserId = permission.LastModifiedByUserId,
                Roles = permission.Roles
            }).ToList();
        }

        public async Task<Permission> GetByIdWithDetailsAsync(Guid id)
        {
            var permission = await _context.Permissions
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (permission == null)
                return null;

            return new Permission
            {
                Id = permission.Id,
                Name = permission.Name,
                Description = permission.Description,
                Group = permission.Group,
                Code = permission.Code,
                IsActive = permission.IsActive,
                IsDeleted = permission.IsDeleted,
                CreatedById = permission.CreatedById,
                CreatedDate = permission.CreatedDate,
                UpdatedByUserId = permission.UpdatedByUserId,
                LastModifiedByUserId = permission.LastModifiedByUserId,
                Roles = permission.Roles
            };
        }

        public async Task<Permission> GetByCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;

            var normalizedCode = code.Trim().ToUpper();
            
            var permission = await _context.Permissions
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Code.ToUpper() == normalizedCode && !x.IsDeleted);

            if (permission == null)
                return null;

            return new Permission
            {
                Id = permission.Id,
                Name = permission.Name,
                Description = permission.Description,
                Group = permission.Group,
                Code = permission.Code,
                IsActive = permission.IsActive,
                IsDeleted = permission.IsDeleted,
                CreatedById = permission.CreatedById,
                CreatedDate = permission.CreatedDate,
                UpdatedByUserId = permission.UpdatedByUserId,
                LastModifiedByUserId = permission.LastModifiedByUserId,
                Roles = permission.Roles
            };
        }

        public async Task<List<Permission>> GetPermissionsByRoleIdAsync(Guid roleId)
        {
            var permissions = await _context.Permissions
                .Include(x => x.Roles)
                .Where(x => x.Roles.Any(r => r.Id == roleId) && !x.IsDeleted)
                .OrderBy(x => x.Group)
                .ThenBy(x => x.Name)
                .ToListAsync();

            return permissions.Select(permission => new Permission
            {
                Id = permission.Id,
                Name = permission.Name,
                Description = permission.Description,
                Group = permission.Group,
                Code = permission.Code,
                IsActive = permission.IsActive,
                IsDeleted = permission.IsDeleted,
                CreatedById = permission.CreatedById,
                CreatedDate = permission.CreatedDate,
                UpdatedByUserId = permission.UpdatedByUserId,
                LastModifiedByUserId = permission.LastModifiedByUserId,
                Roles = permission.Roles
            }).ToList();
        }

        public async Task<bool> IsPermissionCodeExistsAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return false;

            var normalizedCode = code.Trim().ToUpper();
            return await _context.Permissions
                .AnyAsync(x => x.Code.ToUpper() == normalizedCode && !x.IsDeleted);
        }

        public override async Task CreateAsync(Permission permission)
        {
            if (permission == null)
                throw new ArgumentNullException(nameof(permission));

            if (await _context.Permissions.AnyAsync(x => x.Id == permission.Id))
                throw new InvalidOperationException($"Bu ID'ye ({permission.Id}) sahip bir yetki zaten var.");

            if (await IsPermissionCodeExistsAsync(permission.Code))
                throw new InvalidOperationException($"Bu koda ({permission.Code}) sahip bir yetki zaten var.");

            permission.Id = Guid.NewGuid();
            
            await _context.Permissions.AddAsync(permission);
            await _context.SaveChangesAsync();
        }

        public override async Task UpdateAsync(Permission permission)
        {
            if (permission == null)
                throw new ArgumentNullException(nameof(permission));

            try
            {
                var existingPermission = await _context.Permissions
                    .Include(x => x.Roles)
                    .FirstOrDefaultAsync(x => x.Id == permission.Id);

                if (existingPermission == null)
                    throw new InvalidOperationException($"ID'si {permission.Id} olan yetki bulunamadı.");

                // Null kontrollerini yap
                if (permission.UpdatedByUserId == Guid.Empty)
                    permission.UpdatedByUserId = null;
                    
                if (permission.LastModifiedByUserId == Guid.Empty)
                    permission.LastModifiedByUserId = null;

              
                permission.Roles = existingPermission.Roles;

                
                existingPermission.Name = permission.Name;
                existingPermission.Description = permission.Description;
                existingPermission.Group = permission.Group;
                existingPermission.Code = permission.Code;
                existingPermission.IsActive = permission.IsActive;
                existingPermission.LastModifiedDate = permission.LastModifiedDate;
                existingPermission.UpdatedByUserId = permission.UpdatedByUserId;
                existingPermission.LastModifiedByUserId = permission.LastModifiedByUserId;
                
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Yetki güncellenirken bir hata oluştu: {ex.Message}", ex);
            }
        }

        public override async Task RemoveAsync(Permission permission)
        {
            if (permission == null)
                throw new ArgumentNullException(nameof(permission));

            permission.IsDeleted = true;
            await UpdateAsync(permission);
        }
    }
} 