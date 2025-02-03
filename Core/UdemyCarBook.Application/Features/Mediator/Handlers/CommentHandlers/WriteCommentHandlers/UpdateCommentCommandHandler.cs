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
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand>
    {
        private readonly IRepository<Comment> _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public UpdateCommentCommandHandler(IRepository<Comment> repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var comment = await _repository.GetByIdAsync(request.Id);
                if (comment == null)
                    throw new AuFrameWorkException("Yorum bulunamadı", "COMMENT_NOT_FOUND", "NotFound");

                if (string.IsNullOrEmpty(request.Content))
                    throw new AuFrameWorkException("Yorum içeriği boş olamaz", "CONTENT_REQUIRED", "ValidationError");

                if (string.IsNullOrEmpty(request.Name))
                    throw new AuFrameWorkException("İsim boş olamaz", "NAME_REQUIRED", "ValidationError");

                if (string.IsNullOrEmpty(request.Email))
                    throw new AuFrameWorkException("E-posta adresi boş olamaz", "EMAIL_REQUIRED", "ValidationError");

                comment.Content = request.Content;
                comment.Name = request.Name;
                comment.Email = request.Email;
                comment.IsApproved = request.IsApproved;
                comment.NewsId = request.NewsId;
                comment.ParentCommentId = request.ParentCommentId;
                comment.LastModifiedDate = DateTime.UtcNow;

                await _repository.UpdateAsync(comment);
                await _historyService.SaveHistory(comment, "Update");
                
                await _logService.CreateLog(
                    "Yorum Güncelleme",
                    $"'{request.Name}' tarafından yapılan yorum güncellendi",
                    "Update",
                    "Comment"
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "CommentUpdate",
                    $"Yorum güncellenirken hata: {request.Name}"
                );
                throw new AuFrameWorkException(
                    "Yorum güncellenirken bir hata oluştu", 
                    "UPDATE_ERROR",
                    "Error"
                );
            }
        }
    }
} 