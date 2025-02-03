using MediatR;
using System.Collections.Generic;
using UdemyCarBook.Application.Features.Mediator.Results.NewsResults;

namespace UdemyCarBook.Application.Features.Mediator.Queries.NewsQueries
{
    public class SearchNewsQuery : IRequest<List<GetNewsQueryResult>>
    {
        public string SearchTerm { get; set; }
        public string CategoryName { get; set; }
        public string AuthorName { get; set; }
        public string Tag { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsFeatured { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
    }
} 