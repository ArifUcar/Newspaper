using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Commands.CommentCommands;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Application.Interfaces.IService;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Domain.Exceptions;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.CommentHandlers.WriteCommentHandlers
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand>
    {
        private readonly IRepository<Comment> _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public CreateCommentCommandHandler(IRepository<Comment> repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Content))
                    throw new AuFrameWorkException("Yorum içeriği boş olamaz", "CONTENT_REQUIRED", "ValidationError");

                if (string.IsNullOrEmpty(request.Name))
                    throw new AuFrameWorkException("İsim boş olamaz", "NAME_REQUIRED", "ValidationError");

                if (string.IsNullOrEmpty(request.Email))
                    throw new AuFrameWorkException("E-posta adresi boş olamaz", "EMAIL_REQUIRED", "ValidationError");

                var comment = new Comment
                {
                    Id = Guid.NewGuid(),
                    Content = request.Content,
                    Name = request.Name,
                    Email = request.Email,
                    IsApproved = request.IsApproved,
                    NewsId = request.NewsId,
                    ParentCommentId = request.ParentCommentId,
                    IsDeleted = false
                };

                await _repository.CreateAsync(comment);
                await _historyService.SaveHistory(comment, "Create");
                
                await _logService.CreateLog(
                    "Yorum Oluşturma",
                    $"'{request.Name}' tarafından yorum oluşturuldu",
                    "Create",
                    "Comment"
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "CommentCreate",
                    $"Yorum oluşturulurken hata: {request.Name}"
                );
                throw new AuFrameWorkException(
                    "Yorum oluşturulurken bir hata oluştu", 
                    "CREATE_ERROR",
                    "Error"
                );
            }
        }
    }
} 