using UdemyCarBook.Domain.Entities;

namespace UdemyCarBook.Application.Interfaces
{
    public interface IContactRepository : IRepository<Contact>
    {
        Task<List<Contact>> GetAllWithDetailsAsync();
        Task<Contact> GetByIdWithDetailsAsync(Guid id);
        Task<List<Contact>> GetUnreadContactsAsync();
        Task<List<Contact>> GetUnansweredContactsAsync();
    }
} 