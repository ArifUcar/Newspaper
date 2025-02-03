using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Queries.NewsQueries;
using UdemyCarBook.Application.Features.Mediator.Results.NewsResults;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Domain.Exceptions;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.NewsHandlers.ReadNewsHandlers
{
    public class GetNewsByIdQueryHandler : IRequestHandler<GetNewsByIdQuery, GetNewsQueryResult>
    {
        private readonly INewsRepository _repository;

        public GetNewsByIdQueryHandler(INewsRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetNewsQueryResult> Handle(GetNewsByIdQuery request, CancellationToken cancellationToken)
        {
            var news = await _repository.GetByIdWithDetailsAsync(request.Id);
            if (news == null)
                throw new AuFrameWorkException("Haber bulunamadı", "NEWS_NOT_FOUND", "NotFound");

            return new GetNewsQueryResult
            {
                Id = news.Id,
                Title = news.Title,
                Content = news.Content,
                Summary = news.Summary,
                Slug = news.Slug,
                CoverImageUrl = news.CoverImageUrl,
                ViewCount = news.ViewCount,
                IsFeatured = news.IsFeatured,
                IsActive = news.IsActive,
                IsPublished = news.IsPublished,
                PublishDate = news.PublishDate,
                CreatedDate = news.CreatedDate,
                LastModifiedDate = news.LastModifiedDate,
                MetaTitle = news.MetaTitle,
                MetaDescription = news.MetaDescription,
                MetaKeywords = news.MetaKeywords,
                CategoryId = news.CategoryId,
                CategoryName = news.Category?.Name,
                UserId = news.UserId,
                UserName = news.User?.FullName,
                UserImageUrl = news.User?.CoverImageUrl,
                Status = news.Status.ToString(),
                CommentCount = news.Comments?.Count ?? 0,
                Tags = news.Tags?.Select(t => t.Name).ToList() ?? new List<string>(),
                CreatedByUserName = news.CreatedByUser?.UserName,
                LastModifiedByUserName = news.LastModifiedByUser?.UserName
            };
        }
    }
} 