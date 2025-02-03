using MediatR;
using System.Collections.Generic;
using UdemyCarBook.Application.Features.Mediator.Results.UserResults;

namespace UdemyCarBook.Application.Features.Mediator.Queries.UserQueries
{
    public class GetUserQuery : IRequest<List<GetUserQueryResult>>
    {
    }
} 