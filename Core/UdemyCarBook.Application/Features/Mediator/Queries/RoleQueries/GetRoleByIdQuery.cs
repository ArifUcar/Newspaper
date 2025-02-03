using MediatR;
using System;
using UdemyCarBook.Application.Features.Mediator.Results.RoleResults;

namespace UdemyCarBook.Application.Features.Mediator.Queries.RoleQueries
{
    public class GetRoleByIdQuery : IRequest<GetRoleByIdQueryResult>
    {
        public GetRoleByIdQuery(Guid �d)
        {
            Id = �d;
        }

        public Guid Id { get; set; }
    }
} 