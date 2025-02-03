using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UdemyCarBook.Domain.Entities;

namespace UdemyCarBook.Application.Interfaces
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        Task<List<Permission>> GetAllWithDetailsAsync();
        Task<Permission> GetByIdWithDetailsAsync(Guid id);
        Task<Permission> GetByCodeAsync(string code);
        Task<List<Permission>> GetPermissionsByRoleIdAsync(Guid roleId);
        Task<bool> IsPermissionCodeExistsAsync(string code);
    }
} 