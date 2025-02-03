using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Queries.CommentQueries;
using UdemyCarBook.Application.Features.Mediator.Results.CommentResults;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Domain.Exceptions;
using System.Linq;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.CommentHandlers.ReadCommentHandlers
{
    public class GetCommentByIdQueryHandler : IRequestHandler<GetCommentByIdQuery, GetCommentByIdQueryResult>
    {
        private readonly ICommentRepository _repository;

        public GetCommentByIdQueryHandler(ICommentRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetCommentByIdQueryResult> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
        {
            var comment = await _repository.GetByIdWithDetailsAsync(request.Id);

            if (comment == null)
                throw new AuFrameWorkException("Yorum bulunamadı", "COMMENT_NOT_FOUND", "NotFound");

            return new GetCommentByIdQueryResult
            {
                Id = comment.Id,
                Content = comment.Content,
                Name = comment.Name,
                Email = comment.Email,
                IsApproved = comment.IsApproved,
                NewsId = comment.NewsId,
                NewsTitle = comment.News.Title,
                ParentCommentId = comment.ParentCommentId,
                ParentCommentContent = comment.ParentComment?.Content,
                Replies = comment.Replies.Select(r => new CommentReplyDto
                {
                    Id = r.Id,
                    Content = r.Content,
                    Name = r.Name,
                    CreatedDate = r.CreatedDate
                }).ToList(),
                CreatedDate = comment.CreatedDate,
                CreatedByUserName = comment.CreatedByUser?.UserName,
                LastModifiedDate = comment.LastModifiedDate,
                LastModifiedByUserName = comment.LastModifiedByUser?.UserName
            };
        }
    }
} 