using System;
using System.Threading.Tasks;

namespace UdemyCarBook.Application.Interfaces.IService
{
    public interface IPermissionLogService
    {
        Task LogPermissionCreated(string permissionName, string permissionCode, Guid createdByUserId);
        Task LogPermissionUpdated(string permissionName, string permissionCode, Guid updatedByUserId);
        Task LogPermissionDeleted(string permissionName, string permissionCode, Guid deletedByUserId);
        Task LogPermissionStatusChanged(string permissionName, string permissionCode, bool isActive, Guid updatedByUserId);
        Task LogPermissionAssignedToRole(string permissionName, string roleName, Guid assignedByUserId);
        Task LogPermissionRemovedFromRole(string permissionName, string roleName, Guid removedByUserId);
    }
} 