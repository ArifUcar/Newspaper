using MediatR;
using System;
using UdemyCarBook.Application.Features.Mediator.Results.TagResults;

namespace UdemyCarBook.Application.Features.Mediator.Queries.TagQueries
{
    public class GetTagByIdQuery : IRequest<GetTagByIdQueryResult>
    {
        public Guid Id { get; set; }
    }
} 