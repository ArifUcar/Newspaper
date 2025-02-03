using MediatR;
using System.Collections.Generic;
using UdemyCarBook.Application.Features.Mediator.Results.CategoryResults;

namespace UdemyCarBook.Application.Features.Mediator.Queries.CategoryQueries
{
    public class GetCategoryQuery : IRequest<List<GetCategoryQueryResult>>
    {
    }
} 