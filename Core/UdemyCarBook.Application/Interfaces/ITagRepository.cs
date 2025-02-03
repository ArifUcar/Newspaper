using UdemyCarBook.Domain.Entities;

namespace UdemyCarBook.Application.Interfaces
{
    public interface ITagRepository : IRepository<Tag>
    {
        Task<List<Tag>> GetAllWithDetailsAsync();
        Task<Tag> GetByIdWithDetailsAsync(Guid id);
        Task<List<Tag>> GetActiveTagsAsync();
        Task<bool> IsNameExistsAsync(string name);
        Task<List<Tag>> GetTagsByNewsCountAsync(int minNewsCount);
        Task<List<Tag>> GetTagsByNewsIdAsync(Guid newsId);
    }
} 