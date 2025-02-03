using MediatR;
using System.Collections.Generic;
using UdemyCarBook.Application.Features.Mediator.Results.RoleResults;

namespace UdemyCarBook.Application.Features.Mediator.Queries.RoleQueries
{
    public class GetRoleQuery : IRequest<List<GetRoleQueryResult>>
    {
    }
} 