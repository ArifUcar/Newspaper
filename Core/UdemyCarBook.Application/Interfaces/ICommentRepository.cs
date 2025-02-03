using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UdemyCarBook.Domain.Entities;

namespace UdemyCarBook.Application.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<List<Comment>> GetAllWithDetailsAsync();
        Task<Comment> GetByIdWithDetailsAsync(Guid id);
        Task<List<Comment>> GetActiveCommentsAsync();
        Task<List<Comment>> GetCommentsByNewsIdAsync(Guid newsId);
        Task<List<Comment>> GetCommentsByUserIdAsync(Guid userId);
        Task<List<Comment>> GetUnApprovedCommentsAsync();
        Task<List<Comment>> GetApprovedCommentsAsync();
        Task<List<Comment>> GetRepliesByCommentIdAsync(Guid commentId);
        Task<int> GetCommentCountByNewsIdAsync(Guid newsId);
    }
} 