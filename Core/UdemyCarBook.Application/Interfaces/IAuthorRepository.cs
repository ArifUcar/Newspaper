using UdemyCarBook.Domain.Entities;

namespace UdemyCarBook.Application.Interfaces
{
    public interface IAuthorRepository : IRepository<User>
    {
        Task<List<User>> GetAllWithDetailsAsync();
        Task<User> GetByIdWithDetailsAsync(Guid id);
        Task<List<User>> GetActiveAuthorsAsync();
        Task<bool> IsEmailExistsAsync(string email);
        Task<List<User>> GetAuthorsByNewsCountAsync(int minNewsCount);
    }
} 