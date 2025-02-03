using MediatR;
using UdemyCarBook.Application.Features.Mediator.Results.NewsResults;

namespace UdemyCarBook.Application.Features.Mediator.Queries.NewsQueries
{
    public class GetNewsByIdQuery : IRequest<GetNewsQueryResult>
    {
        public Guid Id { get; set; }

        public GetNewsByIdQuery(Guid id)
        {
            Id = id;
        }
    }
} 