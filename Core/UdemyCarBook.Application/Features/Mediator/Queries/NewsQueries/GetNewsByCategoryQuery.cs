using MediatR;
using System.Collections.Generic;
using UdemyCarBook.Application.Features.Mediator.Results.NewsResults;

namespace UdemyCarBook.Application.Features.Mediator.Queries.NewsQueries
{
    public class GetNewsByCategoryQuery : IRequest<List<GetNewsQueryResult>>
    {
        public Guid CategoryId { get; set; }

        public GetNewsByCategoryQuery(Guid categoryId)
        {
            CategoryId = categoryId;
        }
    }
} 