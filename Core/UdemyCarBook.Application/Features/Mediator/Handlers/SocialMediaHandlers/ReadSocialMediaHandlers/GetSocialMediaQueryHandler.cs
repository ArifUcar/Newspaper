using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Queries.SocialMediaQueries;
using UdemyCarBook.Application.Features.Mediator.Results.SocialMediaResults;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Domain.Entities;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.SocialMediaHandlers.ReadSocialMediaHandlers
{
    public class GetSocialMediaQueryHandler : IRequestHandler<GetSocialMediaQuery, List<GetSocialMediaQueryResult>>
    {
        private readonly ISocialMediaRepository _repository;

        public GetSocialMediaQueryHandler(ISocialMediaRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GetSocialMediaQueryResult>> Handle(GetSocialMediaQuery request, CancellationToken cancellationToken)
        {
            var socialMedias = await _repository.GetAllWithDetailsAsync();

            return socialMedias.Select(x => new GetSocialMediaQueryResult
            {
                Id = x.Id,
                Platform = x.Platform,
                Url = x.Url,
                Icon = x.Icon,
                DisplayOrder = x.DisplayOrder,
                IsActive = x.IsActive,
                FollowerCount = x.FollowerCount,
                AccountName = x.AccountName,
                AuthorName = x.User?.FirstName,
                CreatedDate = x.CreatedDate,
                CreatedByUserName = x.CreatedByUser != null ? x.CreatedByUser.UserName : null
            }).ToList();
        }
    }
} 