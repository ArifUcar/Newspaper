using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Queries.NewsletterQueries;
using UdemyCarBook.Application.Features.Mediator.Results.NewsletterResults;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Domain.Exceptions;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.NewsletterHandlers.ReadNewsletterHandlers
{
    public class GetNewsletterByIdQueryHandler : IRequestHandler<GetNewsletterByIdQuery, GetNewsletterByIdQueryResult>
    {
        private readonly INewsletterRepository _repository;

        public GetNewsletterByIdQueryHandler(INewsletterRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetNewsletterByIdQueryResult> Handle(GetNewsletterByIdQuery request, CancellationToken cancellationToken)
        {
            var newsletter = await _repository.GetByIdWithDetailsAsync(request.Id);

            if (newsletter == null)
                throw new AuFrameWorkException("Bülten aboneliği bulunamadı", "NEWSLETTER_NOT_FOUND", "NotFound");

            return new GetNewsletterByIdQueryResult
            {
                Id = newsletter.Id,
                Email = newsletter.Email,
                IsActive = newsletter.IsActive,
                SubscriptionDate = newsletter.SubscriptionDate,
                UnsubscribeDate = newsletter.UnsubscribeDate,
                CreatedDate = newsletter.CreatedDate,
                CreatedByUserName = newsletter.CreatedByUser != null ? newsletter.CreatedByUser.UserName : null,
                LastModifiedDate = newsletter.LastModifiedDate,
                LastModifiedByUserName = newsletter.LastModifiedByUser != null ? newsletter.LastModifiedByUser.UserName : null
            };
        }
    }
} 