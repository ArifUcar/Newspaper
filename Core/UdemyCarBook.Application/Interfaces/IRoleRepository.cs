using System.Threading.Tasks;
using UdemyCarBook.Domain.Entities;

namespace UdemyCarBook.Application.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<List<Role>> GetAllWithDetailsAsync();
        Task<Role> GetByIdWithDetailsAsync(Guid id);
        Task<List<Role>> GetRolesByUserIdAsync(Guid userId);
        Task<bool> IsRoleNameExistsAsync(string name);
        Task<Role> GetByNameAsync(string name);
    }
} 