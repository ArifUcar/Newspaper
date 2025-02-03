using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Queries.NewsletterQueries;
using UdemyCarBook.Application.Features.Mediator.Results.NewsletterResults;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Domain.Entities;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.NewsletterHandlers.ReadNewsletterHandlers
{
    public class GetNewsletterQueryHandler : IRequestHandler<GetNewsletterQuery, List<GetNewsletterQueryResult>>
    {
        private readonly INewsletterRepository _repository;

        public GetNewsletterQueryHandler(INewsletterRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GetNewsletterQueryResult>> Handle(GetNewsletterQuery request, CancellationToken cancellationToken)
        {
            var newsletters = await _repository.GetAllWithDetailsAsync();

            return newsletters.Select(x => new GetNewsletterQueryResult
            {
                Id = x.Id,
                Email = x.Email,
                IsActive = x.IsActive,
                SubscriptionDate = x.SubscriptionDate,
                UnsubscribeDate = x.UnsubscribeDate,
                CreatedDate = x.CreatedDate,
                CreatedByUserName = x.CreatedByUser != null ? x.CreatedByUser.UserName : null
            }).ToList();
        }
    }
} 