using MediatR;
using System;
using UdemyCarBook.Application.Features.Mediator.Results.UserResults;

namespace UdemyCarBook.Application.Features.Mediator.Queries.UserQueries
{
    public class GetUserByIdQuery : IRequest<GetUserByIdQueryResult>
    {
        public GetUserByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
} 