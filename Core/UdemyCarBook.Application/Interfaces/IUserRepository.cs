using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UdemyCarBook.Domain.Entities;

namespace UdemyCarBook.Application.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        DbContext Context { get; }
        Task<List<User>> GetAllWithDetailsAsync();
        Task<User> GetByIdWithDetailsAsync(Guid id);
        Task<List<User>> GetActiveUsersAsync();
        Task<bool> IsEmailExistsAsync(string email);
        Task<bool> IsUsernameExistsAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByUsernameAsync(string username);
        Task<List<User>> GetUsersByRoleAsync(string roleName);
        Task<bool> IsInRoleAsync(Guid userId, string roleName);
    }
} 