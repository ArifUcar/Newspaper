using MediatR;
using System;
using UdemyCarBook.Application.Features.Mediator.Results.ContactResults;

namespace UdemyCarBook.Application.Features.Mediator.Queries.ContactQueries
{
    public class GetContactByIdQuery : IRequest<GetContactByIdQueryResult>
    {
        public Guid Id { get; set; }
    }
} 