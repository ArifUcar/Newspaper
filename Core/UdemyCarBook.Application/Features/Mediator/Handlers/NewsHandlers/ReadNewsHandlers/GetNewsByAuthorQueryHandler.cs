using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Queries.NewsQueries;
using UdemyCarBook.Application.Features.Mediator.Results.NewsResults;
using UdemyCarBook.Application.Interfaces;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.NewsHandlers.ReadNewsHandlers
{
    public class GetNewsByAuthorQueryHandler : IRequestHandler<GetNewsByAuthorQuery, List<GetNewsQueryResult>>
    {
        private readonly INewsRepository _repository;

        public GetNewsByAuthorQueryHandler(INewsRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GetNewsQueryResult>> Handle(GetNewsByAuthorQuery request, CancellationToken cancellationToken)
        {
            var news = await _repository.GetAllWithDetailsAsync();
            var authorNews = news.Where(x => x.UserId == request.AuthorId).ToList();

            return authorNews.Select(x => new GetNewsQueryResult
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                Summary = x.Summary,
                Slug = x.Slug,
                CoverImageUrl = x.CoverImageUrl,
                ViewCount = x.ViewCount,
                IsFeatured = x.IsFeatured,
                IsActive = x.IsActive,
                IsPublished = x.IsPublished,
                PublishDate = x.PublishDate,
                CreatedDate = x.CreatedDate,
                LastModifiedDate = x.LastModifiedDate,
                MetaTitle = x.MetaTitle,
                MetaDescription = x.MetaDescription,
                MetaKeywords = x.MetaKeywords,
                CategoryId = x.CategoryId,
                CategoryName = x.Category?.Name,
                UserId = x.UserId,
                UserName = x.User?.FullName,
                UserImageUrl = x.User?.CoverImageUrl,
                Status = x.Status.ToString(),
                CommentCount = x.Comments?.Count ?? 0,
                Tags = x.Tags?.Select(t => t.Name).ToList() ?? new List<string>(),
                CreatedByUserName = x.CreatedByUser?.UserName,
                LastModifiedByUserName = x.LastModifiedByUser?.UserName
            }).ToList();
        }
    }
} 