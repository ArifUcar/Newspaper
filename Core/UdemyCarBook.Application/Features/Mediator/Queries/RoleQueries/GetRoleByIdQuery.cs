using MediatR;
using System;
using UdemyCarBook.Application.Features.Mediator.Results.RoleResults;

namespace UdemyCarBook.Application.Features.Mediator.Queries.RoleQueries
{
    public class GetRoleByIdQuery : IRequest<GetRoleByIdQueryResult>
    {
        public GetRoleByIdQuery(Guid ýd)
        {
            Id = ýd;
        }

        public Guid Id { get; set; }
    }
} 