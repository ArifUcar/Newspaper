using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UdemyCarBook.Domain.Entities;

namespace UdemyCarBook.Application.Interfaces
{
    public interface INewsRepository : IRepository<News>
    {
        Task<List<News>> GetAllWithDetailsAsync();
        Task<News> GetByIdWithDetailsAsync(Guid id);
        Task<List<News>> GetNewsByCategoryAsync(Guid categoryId);
        Task<List<News>> GetNewsByUserAsync(Guid userId);
        Task<List<News>> GetNewsByTagAsync(Guid tagId);
        Task<List<News>> SearchNewsAsync(string keyword);
        Task<List<News>> GetLatestNewsAsync(int count);
        Task<List<News>> GetPopularNewsAsync(int count);
        Task<List<News>> GetFeaturedNewsAsync();
        Task<bool> IsNewsExistsAsync(Guid id);
        Task<bool> IsNewsSlugExistsAsync(string slug);
        Task IncrementViewCountAsync(Guid id);
    }
} 