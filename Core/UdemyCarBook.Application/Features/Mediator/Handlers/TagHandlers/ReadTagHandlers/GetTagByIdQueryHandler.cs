using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Queries.TagQueries;
using UdemyCarBook.Application.Features.Mediator.Results.TagResults;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Domain.Exceptions;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.TagHandlers.ReadTagHandlers
{
    public class GetTagByIdQueryHandler : IRequestHandler<GetTagByIdQuery, GetTagByIdQueryResult>
    {
        private readonly ITagRepository _repository;

        public GetTagByIdQueryHandler(ITagRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetTagByIdQueryResult> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
        {
            var tag = await _repository.GetByIdWithDetailsAsync(request.Id);

            if (tag == null)
                throw new AuFrameWorkException("Etiket bulunamadı", "TAG_NOT_FOUND", "NotFound");

            return new GetTagByIdQueryResult
            {
                Id = tag.Id,
                Name = tag.Name,
              
                News = tag.News?.Where(n => !n.IsDeleted).Select(n => new TagNewsDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    PublishDate = n.PublishDate,
                    CategoryName = n.Category?.Name,
                    AuthorName = n.User?.FirstName
                }).ToList(),
                CreatedDate = tag.CreatedDate,
                CreatedByUserName = tag.CreatedByUser != null ? tag.CreatedByUser.UserName : null,
                LastModifiedDate = tag.LastModifiedDate,
                LastModifiedByUserName = tag.LastModifiedByUser != null ? tag.LastModifiedByUser.UserName : null
            };
        }
    }
} 