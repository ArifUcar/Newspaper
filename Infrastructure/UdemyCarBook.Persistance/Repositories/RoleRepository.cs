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
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private readonly NewsContext _context;

        public RoleRepository(NewsContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Role>> GetAllWithDetailsAsync()
        {
            var roles = await _context.Roles
                .Include(x => x.Users)
                .Include(x => x.CreatedByUser)
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Name)
                .Select(x => new Role
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    IsActive = x.IsActive,
                    IsDeleted = x.IsDeleted,
                    CreatedById = x.CreatedById,
                    CreatedDate = x.CreatedDate,
                    UpdatedByUserId = x.UpdatedByUserId.HasValue ? x.UpdatedByUserId : null,
                    LastModifiedByUserId = x.LastModifiedByUserId.HasValue ? x.LastModifiedByUserId : null,
                    Users = x.Users,
                    CreatedByUser = x.CreatedByUser,
                    UpdatedByUser = x.UpdatedByUserId.HasValue ? _context.Users.FirstOrDefault(u => u.Id == x.UpdatedByUserId) : null,
                    LastModifiedByUser = x.LastModifiedByUserId.HasValue ? _context.Users.FirstOrDefault(u => u.Id == x.LastModifiedByUserId) : null
                })
                .ToListAsync();

            return roles;
        }

        public async Task<Role> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Roles
                .Include(x => x.Users)
                .Include(x => x.CreatedByUser)
                .Where(x => x.Id == id && !x.IsDeleted)
                .Select(x => new Role
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    IsActive = x.IsActive,
                    IsDeleted = x.IsDeleted,
                    CreatedById = x.CreatedById,
                    CreatedDate = x.CreatedDate,
                    UpdatedByUserId = x.UpdatedByUserId.HasValue ? x.UpdatedByUserId : null,
                    LastModifiedByUserId = x.LastModifiedByUserId.HasValue ? x.LastModifiedByUserId : null,
                    Users = x.Users,
                    CreatedByUser = x.CreatedByUser,
                    UpdatedByUser = x.UpdatedByUserId.HasValue ? _context.Users.FirstOrDefault(u => u.Id == x.UpdatedByUserId) : null,
                    LastModifiedByUser = x.LastModifiedByUserId.HasValue ? _context.Users.FirstOrDefault(u => u.Id == x.LastModifiedByUserId) : null
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<Role>> GetRolesByUserIdAsync(Guid userId)
        {
            return await _context.Roles
                .Include(r => r.Users)
                .Include(x => x.CreatedByUser)
                .Where(r => r.Users.Any(u => u.Id == userId) && !r.IsDeleted)
                .OrderBy(x => x.Name)
                .Select(x => new Role
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    IsActive = x.IsActive,
                    IsDeleted = x.IsDeleted,
                    CreatedById = x.CreatedById,
                    CreatedDate = x.CreatedDate,
                    UpdatedByUserId = x.UpdatedByUserId.HasValue ? x.UpdatedByUserId : null,
                    LastModifiedByUserId = x.LastModifiedByUserId.HasValue ? x.LastModifiedByUserId : null,
                    Users = x.Users,
                    CreatedByUser = x.CreatedByUser,
                    UpdatedByUser = x.UpdatedByUserId.HasValue ? _context.Users.FirstOrDefault(u => u.Id == x.UpdatedByUserId) : null,
                    LastModifiedByUser = x.LastModifiedByUserId.HasValue ? _context.Users.FirstOrDefault(u => u.Id == x.LastModifiedByUserId) : null
                })
                .ToListAsync();
        }

        public async Task<bool> IsRoleNameExistsAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            var normalizedName = name.Trim().ToUpper();
            return await _context.Roles
                .AnyAsync(r => r.Name.ToUpper() == normalizedName && !r.IsDeleted);
        }

        public async Task<Role> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            var normalizedName = name.Trim().ToUpper();
            
            return await _context.Roles
                .Include(x => x.Users)
                .Include(x => x.CreatedByUser)
                .Where(r => r.Name.ToUpper() == normalizedName && !r.IsDeleted)
                .Select(x => new Role
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    IsActive = x.IsActive,
                    IsDeleted = x.IsDeleted,
                    CreatedById = x.CreatedById,
                    CreatedDate = x.CreatedDate,
                    UpdatedByUserId = x.UpdatedByUserId.HasValue ? x.UpdatedByUserId : null,
                    LastModifiedByUserId = x.LastModifiedByUserId.HasValue ? x.LastModifiedByUserId : null,
                    Users = x.Users,
                    CreatedByUser = x.CreatedByUser,
                    UpdatedByUser = x.UpdatedByUserId.HasValue ? _context.Users.FirstOrDefault(u => u.Id == x.UpdatedByUserId) : null,
                    LastModifiedByUser = x.LastModifiedByUserId.HasValue ? _context.Users.FirstOrDefault(u => u.Id == x.LastModifiedByUserId) : null
                })
                .FirstOrDefaultAsync();
        }

        public override async Task CreateAsync(Role role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            if (await _context.Roles.AnyAsync(r => r.Id == role.Id))
                throw new InvalidOperationException($"Bu ID'ye ({role.Id}) sahip bir rol zaten var.");

            if (await IsRoleNameExistsAsync(role.Name))
                throw new InvalidOperationException($"Bu isme ({role.Name}) sahip bir rol zaten var.");

            role.Id = Guid.NewGuid();
            
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
        }

        public override async Task UpdateAsync(Role role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            try
            {
                var existingRole = await _context.Roles
                    .Include(r => r.Users)
                    .FirstOrDefaultAsync(r => r.Id == role.Id);

                if (existingRole == null)
                    throw new InvalidOperationException($"ID'si {role.Id} olan rol bulunamadı.");

                // Null kontrollerini yap
                if (role.UpdatedByUserId == Guid.Empty)
                    role.UpdatedByUserId = null;
                    
                if (role.LastModifiedByUserId == Guid.Empty)
                    role.LastModifiedByUserId = null;

                // Mevcut kullanıcı ilişkilerini koru
                role.Users = existingRole.Users;

                // Entity'nin diğer özelliklerini güncelle
                existingRole.Name = role.Name;
                existingRole.Description = role.Description;
                existingRole.IsActive = role.IsActive;
                existingRole.LastModifiedDate = role.LastModifiedDate;
                existingRole.UpdatedByUserId = role.UpdatedByUserId;
                existingRole.LastModifiedByUserId = role.LastModifiedByUserId;
                
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Rol güncellenirken bir hata oluştu: {ex.Message}", ex);
            }
        }

        public override async Task RemoveAsync(Role role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            role.IsDeleted = true;
            await UpdateAsync(role);
        }
    }
} 