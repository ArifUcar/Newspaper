using MediatR;
using System.Collections.Generic;
using UdemyCarBook.Application.Features.Mediator.Results.ContactResults;

namespace UdemyCarBook.Application.Features.Mediator.Queries.ContactQueries
{
    public class GetContactQuery : IRequest<List<GetContactQueryResult>>
    {
    }
} 