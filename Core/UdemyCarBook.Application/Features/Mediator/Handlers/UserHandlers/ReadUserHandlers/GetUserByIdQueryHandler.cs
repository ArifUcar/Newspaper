using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Queries.UserQueries;
using UdemyCarBook.Application.Features.Mediator.Results.UserResults;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Domain.Exceptions;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.UserHandlers.ReadUserHandlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdQueryResult>
    {
        private readonly IUserRepository _repository;

        public GetUserByIdQueryHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetUserByIdQueryResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdWithDetailsAsync(request.Id);

            if (user == null)
                throw new AuFrameWorkException("Kullanıcı bulunamadı", "USER_NOT_FOUND", "NotFound");

            return new GetUserByIdQueryResult
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                Roles = user.Roles?.Select(r => r.Name).ToList(),
                CreatedNews = user.CreatedNews?.Where(n => !n.IsDeleted).Select(n => new NewsInfo
                {
                    Id = n.Id,
                    Title = n.Title,
                    CreatedDate = n.CreatedDate
                }).ToList(),
                UpdatedNews = user.UpdatedNews?.Where(n => !n.IsDeleted).Select(n => new NewsInfo
                {
                    Id = n.Id,
                    Title = n.Title,
                    CreatedDate = n.CreatedDate
                }).ToList(),
                CreatedComments = user.CreatedComments?.Where(c => !c.IsDeleted).Select(c => new CommentInfo
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedDate = c.CreatedDate,
                    NewsTitle = c.News?.Title,
                    IsApproved = c.IsApproved
                }).ToList(),
                UpdatedComments = user.UpdatedComments?.Where(c => !c.IsDeleted).Select(c => new CommentInfo
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedDate = c.CreatedDate,
                    NewsTitle = c.News?.Title,
                    IsApproved = c.IsApproved
                }).ToList(),
                CreatedDate = user.CreatedDate,
                CreatedByUserName = user.CreatedByUser?.UserName,
                LastModifiedDate = user.LastModifiedDate,
                LastModifiedByUserName = user.LastModifiedByUser?.UserName
            };
        }
    }
} 