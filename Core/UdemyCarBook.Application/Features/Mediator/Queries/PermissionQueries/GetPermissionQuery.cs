using MediatR;
using System.Collections.Generic;
using UdemyCarBook.Application.Features.Mediator.Results.PermissionResults;

namespace UdemyCarBook.Application.Features.Mediator.Queries.PermissionQueries
{
    public class GetPermissionQuery : IRequest<List<GetPermissionQueryResult>>
    {
    }
} 