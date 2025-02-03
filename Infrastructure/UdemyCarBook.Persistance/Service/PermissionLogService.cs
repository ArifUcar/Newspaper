using System;
using System.Threading.Tasks;
using UdemyCarBook.Application.Interfaces.IService;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Persistance.Context;

namespace UdemyCarBook.Persistance.Service
{
    public class PermissionLogService : IPermissionLogService
    {
        private readonly NewsContext _context;
        private readonly IUserService _userService;

        public PermissionLogService(NewsContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task LogPermissionCreated(string permissionName, string permissionCode, Guid createdByUserId)
        {
            var userName = await _userService.GetUserNameAsync(createdByUserId);
            var log = new Log
            {
                Id = Guid.NewGuid(),
                Title = "Yetki Oluşturma",
                Message = $"'{permissionName}' ({permissionCode}) yetkisi {userName} tarafından oluşturuldu",
                Type = "Create",
                Location = "Permission",
                CreatedDate = DateTime.UtcNow
            };

            await _context.Logs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task LogPermissionUpdated(string permissionName, string permissionCode, Guid updatedByUserId)
        {
            var userName = await _userService.GetUserNameAsync(updatedByUserId);
            var log = new Log
            {
                Id = Guid.NewGuid(),
                Title = "Yetki Güncelleme",
                Message = $"'{permissionName}' ({permissionCode}) yetkisi {userName} tarafından güncellendi",
                Type = "Update",
                Location = "Permission",
                CreatedDate = DateTime.UtcNow
            };

            await _context.Logs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task LogPermissionDeleted(string permissionName, string permissionCode, Guid deletedByUserId)
        {
            var userName = await _userService.GetUserNameAsync(deletedByUserId);
            var log = new Log
            {
                Id = Guid.NewGuid(),
                Title = "Yetki Silme",
                Message = $"'{permissionName}' ({permissionCode}) yetkisi {userName} tarafından silindi",
                Type = "Delete",
                Location = "Permission",
                CreatedDate = DateTime.UtcNow
            };

            await _context.Logs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task LogPermissionStatusChanged(string permissionName, string permissionCode, bool isActive, Guid updatedByUserId)
        {
            var userName = await _userService.GetUserNameAsync(updatedByUserId);
            var status = isActive ? "aktif" : "pasif";
            var log = new Log
            {
                Id = Guid.NewGuid(),
                Title = "Yetki Durum Değişikliği",
                Message = $"'{permissionName}' ({permissionCode}) yetkisi {userName} tarafından {status} yapıldı",
                Type = "Update",
                Location = "Permission",
                CreatedDate = DateTime.UtcNow
            };

            await _context.Logs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task LogPermissionAssignedToRole(string permissionName, string roleName, Guid assignedByUserId)
        {
            var userName = await _userService.GetUserNameAsync(assignedByUserId);
            var log = new Log
            {
                Id = Guid.NewGuid(),
                Title = "Yetki Rol Ataması",
                Message = $"'{permissionName}' yetkisi '{roleName}' rolüne {userName} tarafından atandı",
                Type = "Create",
                Location = "RolePermission",
                CreatedDate = DateTime.UtcNow
            };

            await _context.Logs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task LogPermissionRemovedFromRole(string permissionName, string roleName, Guid removedByUserId)
        {
            var userName = await _userService.GetUserNameAsync(removedByUserId);
            var log = new Log
            {
                Id = Guid.NewGuid(),
                Title = "Yetki Rol Kaldırma",
                Message = $"'{permissionName}' yetkisi '{roleName}' rolünden {userName} tarafından kaldırıldı",
                Type = "Delete",
                Location = "RolePermission",
                CreatedDate = DateTime.UtcNow
            };

            await _context.Logs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}