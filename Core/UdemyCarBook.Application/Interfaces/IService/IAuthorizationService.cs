using System;
using System.Threading.Tasks;

namespace UdemyCarBook.Application.Interfaces.IService
{
    public interface IPermissionAuthorizationService
    {
        Task<bool> HasPermissionAsync(Guid userId, string permissionCode);
        Task<bool> HasAnyPermissionAsync(Guid userId, params string[] permissionCodes);
        Task<bool> HasAllPermissionsAsync(Guid userId, params string[] permissionCodes);
    }
} 