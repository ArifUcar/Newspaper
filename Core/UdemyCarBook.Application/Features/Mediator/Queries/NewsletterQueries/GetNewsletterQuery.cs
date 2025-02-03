using MediatR;
using UdemyCarBook.Application.Features.Mediator.Results.NewsletterResults;

namespace UdemyCarBook.Application.Features.Mediator.Queries.NewsletterQueries
{
    public class GetNewsletterQuery : IRequest<List<GetNewsletterQueryResult>>
    {
    }
} 