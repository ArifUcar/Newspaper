using UdemyCarBook.Domain.Entities;

namespace UdemyCarBook.Application.Interfaces
{
    public interface ISocialMediaRepository : IRepository<SocialMedia>
    {
        Task<List<SocialMedia>> GetAllWithDetailsAsync();
        Task<SocialMedia> GetByIdWithDetailsAsync(Guid id);
        Task<List<SocialMedia>> GetByAuthorIdAsync(Guid authorId);
        Task<List<SocialMedia>> GetActiveAccountsAsync();
        Task<bool> IsPlatformExistsForAuthorAsync(string platform, Guid? authorId);
    }
} 