using MediatR;
using System;
using UdemyCarBook.Application.Features.Mediator.Results.CategoryResults;

namespace UdemyCarBook.Application.Features.Mediator.Queries.CategoryQueries
{
    public class GetCategoryByIdQuery : IRequest<GetCategoryQueryResult>
    {
        public Guid Id { get; set; }

        public GetCategoryByIdQuery(Guid id)
        {
            Id = id;
        }
    }
} 