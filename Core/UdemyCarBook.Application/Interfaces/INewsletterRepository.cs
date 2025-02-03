using UdemyCarBook.Domain.Entities;

namespace UdemyCarBook.Application.Interfaces
{
    public interface INewsletterRepository : IRepository<Newsletter>
    {
        Task<List<Newsletter>> GetAllWithDetailsAsync();
        Task<Newsletter> GetByIdWithDetailsAsync(Guid id);
        Task<List<Newsletter>> GetActiveSubscribersAsync();
        Task<bool> IsEmailSubscribedAsync(string email);
    }
} 