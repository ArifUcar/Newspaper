using MediatR;
using System.Collections.Generic;
using UdemyCarBook.Application.Features.Mediator.Results.NewsResults;

namespace UdemyCarBook.Application.Features.Mediator.Queries.NewsQueries
{
    public class GetPopularNewsQuery : IRequest<List<GetNewsQueryResult>>
    {
        public int Count { get; }

        public GetPopularNewsQuery(int count = 5)
        {
            Count = count;
        }
    }
} 