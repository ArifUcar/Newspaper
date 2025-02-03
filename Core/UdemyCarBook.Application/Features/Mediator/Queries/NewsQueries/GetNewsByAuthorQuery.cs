using MediatR;
using System.Collections.Generic;
using UdemyCarBook.Application.Features.Mediator.Results.NewsResults;

namespace UdemyCarBook.Application.Features.Mediator.Queries.NewsQueries
{
    public class GetNewsByAuthorQuery : IRequest<List<GetNewsQueryResult>>
    {
        public Guid AuthorId { get; set; }

        public GetNewsByAuthorQuery(Guid authorId)
        {
            AuthorId = authorId;
        }
    }
} 