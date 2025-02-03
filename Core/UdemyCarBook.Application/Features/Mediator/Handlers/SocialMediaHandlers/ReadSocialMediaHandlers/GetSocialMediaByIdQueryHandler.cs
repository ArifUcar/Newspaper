using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Queries.SocialMediaQueries;
using UdemyCarBook.Application.Features.Mediator.Results.SocialMediaResults;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Domain.Exceptions;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.SocialMediaHandlers.ReadSocialMediaHandlers
{
    public class GetSocialMediaByIdQueryHandler : IRequestHandler<GetSocialMediaByIdQuery, GetSocialMediaByIdQueryResult>
    {
        private readonly ISocialMediaRepository _repository;

        public GetSocialMediaByIdQueryHandler(ISocialMediaRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetSocialMediaByIdQueryResult> Handle(GetSocialMediaByIdQuery request, CancellationToken cancellationToken)
        {
            var socialMedia = await _repository.GetByIdWithDetailsAsync(request.Id);

            if (socialMedia == null)
                throw new AuFrameWorkException("Sosyal medya hesabı bulunamadı", "SOCIAL_MEDIA_NOT_FOUND", "NotFound");

            return new GetSocialMediaByIdQueryResult
            {
                Id = socialMedia.Id,
                Platform = socialMedia.Platform,
                Url = socialMedia.Url,
                Icon = socialMedia.Icon,
                DisplayOrder = socialMedia.DisplayOrder,
                IsActive = socialMedia.IsActive,
                FollowerCount = socialMedia.FollowerCount,
                AccountName = socialMedia.AccountName,
                AuthorId = socialMedia.UserId,
                AuthorName = socialMedia.User?.FirstName,
                CreatedDate = socialMedia.CreatedDate,
                CreatedByUserName = socialMedia.CreatedByUser != null ? socialMedia.CreatedByUser.UserName : null,
                LastModifiedDate = socialMedia.LastModifiedDate,
                LastModifiedByUserName = socialMedia.LastModifiedByUser != null ? socialMedia.LastModifiedByUser.UserName : null
            };
        }
    }
} 