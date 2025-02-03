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
    public class GetPopularNewsQueryHandler : IRequestHandler<GetPopularNewsQuery, List<GetNewsQueryResult>>
    {
        private readonly INewsRepository _repository;

        public GetPopularNewsQueryHandler(INewsRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GetNewsQueryResult>> Handle(GetPopularNewsQuery request, CancellationToken cancellationToken)
        {
            var news = await _repository.GetPopularNewsAsync(request.Count);

            return news.Select(x => new GetNewsQueryResult
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                CoverImageUrl = x.CoverImageUrl,
                CreatedDate = x.CreatedDate,
                UserName = x.User?.FullName,
                CategoryName = x.Category?.Name,
                ViewCount = x.ViewCount,
                Tags = x.Tags?.Select(t => t.Name).ToList() ?? new List<string>(),
                IsActive = x.IsActive,
                IsFeatured = x.IsFeatured
            }).ToList();
        }
    }
} 