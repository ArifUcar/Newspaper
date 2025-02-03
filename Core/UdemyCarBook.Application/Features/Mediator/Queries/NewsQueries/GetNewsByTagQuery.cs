using MediatR;
using System.Collections.Generic;
using UdemyCarBook.Application.Features.Mediator.Results.NewsResults;

namespace UdemyCarBook.Application.Features.Mediator.Queries.NewsQueries
{
    public class GetNewsByTagQuery : IRequest<List<GetNewsQueryResult>>
    {
        public Guid TagId { get; set; }

        public GetNewsByTagQuery(Guid tagId)
        {
            TagId = tagId;
        }
    }
} 