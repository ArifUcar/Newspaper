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
    public class SearchNewsQueryHandler : IRequestHandler<SearchNewsQuery, List<GetNewsQueryResult>>
    {
        private readonly INewsRepository _repository;

        public SearchNewsQueryHandler(INewsRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GetNewsQueryResult>> Handle(SearchNewsQuery request, CancellationToken cancellationToken)
        {
            var news = await _repository.GetAllWithDetailsAsync();
            var query = news.AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                query = query.Where(x => 
                    x.Title.ToLower().Contains(searchTerm) || 
                    x.Content.ToLower().Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(request.CategoryName))
            {
                query = query.Where(x => x.Category.Name.ToLower() == request.CategoryName.ToLower());
            }

            if (!string.IsNullOrEmpty(request.AuthorName))
            {
                query = query.Where(x => x.User.FullName.ToLower().Contains(request.AuthorName.ToLower()));
            }

            if (!string.IsNullOrEmpty(request.Tag))
            {
                query = query.Where(x => x.Tags.Any(t => t.Name.ToLower() == request.Tag.ToLower()));
            }

            if (request.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == request.IsActive.Value);
            }

            if (request.IsFeatured.HasValue)
            {
                query = query.Where(x => x.IsFeatured == request.IsFeatured.Value);
            }

            if (request.Skip.HasValue)
            {
                query = query.Skip(request.Skip.Value);
            }

            if (request.Take.HasValue)
            {
                query = query.Take(request.Take.Value);
            }

            var filteredNews = query.ToList();

            return filteredNews.Select(x => new GetNewsQueryResult
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