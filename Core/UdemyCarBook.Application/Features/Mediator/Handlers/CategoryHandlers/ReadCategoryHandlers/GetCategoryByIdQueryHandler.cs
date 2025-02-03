using MediatR;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Queries.CategoryQueries;
using UdemyCarBook.Application.Features.Mediator.Results.CategoryResults;
using UdemyCarBook.Application.Features.Mediator.Results.NewsResults;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Domain.Exceptions;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.CategoryHandlers.ReadCategoryHandlers
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, GetCategoryQueryResult>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<GetCategoryQueryResult> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdWithDetailsAsync(request.Id);
            if (category == null)
            {
                throw new AuFrameWorkException(
                    "Kategori bulunamadı",
                    "CATEGORY_NOT_FOUND",
                    "NotFound"
                );
            }

            return new GetCategoryQueryResult
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
            };
        }
    }
} 