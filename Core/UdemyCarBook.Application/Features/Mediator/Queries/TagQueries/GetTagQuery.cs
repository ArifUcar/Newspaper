using MediatR;
using System.Collections.Generic;
using UdemyCarBook.Application.Features.Mediator.Results.TagResults;

namespace UdemyCarBook.Application.Features.Mediator.Queries.TagQueries
{
    public class GetTagQuery : IRequest<List<GetTagQueryResult>>
    {
    }
} 