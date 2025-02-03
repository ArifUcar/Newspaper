using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Queries.TagQueries;
using UdemyCarBook.Application.Features.Mediator.Results.TagResults;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Domain.Entities;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.TagHandlers.ReadTagHandlers
{
    public class GetTagQueryHandler : IRequestHandler<GetTagQuery, List<GetTagQueryResult>>
    {
        private readonly ITagRepository _repository;

        public GetTagQueryHandler(ITagRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GetTagQueryResult>> Handle(GetTagQuery request, CancellationToken cancellationToken)
        {
            var tags = await _repository.GetAllWithDetailsAsync();

            return tags.Select(x => new GetTagQueryResult
            {
                Id = x.Id,
                Name = x.Name,
        
                NewsCount = x.News?.Count(n => !n.IsDeleted) ?? 0,
                CreatedDate = x.CreatedDate,
                CreatedByUserName = x.CreatedByUser != null ? x.CreatedByUser.UserName : null
            }).ToList();
        }
    }
} 