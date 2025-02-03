using UdemyCarBook.Domain.Entities;

namespace UdemyCarBook.Application.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<List<Category>> GetAllWithDetailsAsync();
        Task<Category> GetByIdWithDetailsAsync(Guid id);
        Task<bool> IsCategoryExistsAsync(string name);
    }
} 