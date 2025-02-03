using System;
using System.Threading.Tasks;
using UdemyCarBook.Domain.Entities;

namespace UdemyCarBook.Application.Interfaces.IService
{
    public interface IUserService
    {
        Task<User> GetCurrentUserAsync();
        Task<Guid> GetCurrentUserIdAsync();
        Task<string> GetUserNameAsync(Guid userId);
        Task<bool> IsInRoleAsync(Guid userId, string roleName);
    }
} 