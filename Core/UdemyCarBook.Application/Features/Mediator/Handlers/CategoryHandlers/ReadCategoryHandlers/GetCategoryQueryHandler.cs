using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Queries.CategoryQueries;
using UdemyCarBook.Application.Features.Mediator.Results.CategoryResults;
using UdemyCarBook.Application.Features.Mediator.Results.NewsResults;
using UdemyCarBook.Application.Interfaces;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.CategoryHandlers.ReadCategoryHandlers
{
    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, List<GetCategoryQueryResult>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<GetCategoryQueryResult>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAllWithDetailsAsync();
            return categories.Select(category => new GetCategoryQueryResult
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IconUrl = category.IconUrl,
                CreatedDate = category.CreatedDate,
                CreatedByUserName = category.CreatedByUser?.UserName,
                LastModifiedDate = category.LastModifiedDate,
                LastModifiedByUserName = category.LastModifiedByUser?.UserName,
                News = category.News?.Select(n => new GetNewsQueryResult
                {
                    Id = n.Id,
                    Title = n.Title,
                    Summary = n.Summary,
                    CoverImageUrl = n.CoverImageUrl,
                    PublishDate = n.PublishDate,
                    ViewCount = n.ViewCount,
                    IsFeatured = n.IsFeatured,
                    IsActive = n.IsActive,
                    IsPublished = n.IsPublished,
                    CategoryName = category.Name,
                    UserName = n.User?.UserName
                }).ToList()
            }).ToList();
        }
    }
} 