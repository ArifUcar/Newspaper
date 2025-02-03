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
    public class SoftDeleteCommentCommandHandler : IRequestHandler<SoftDeleteCommentCommand>
    {
        private readonly IRepository<Comment> _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public SoftDeleteCommentCommandHandler(IRepository<Comment> repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task Handle(SoftDeleteCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var comment = await _repository.GetByIdAsync(request.Id);
                if (comment == null)
                    throw new AuFrameWorkException("Yorum bulunamadı", "COMMENT_NOT_FOUND", "NotFound");

                comment.IsDeleted = true;
                comment.LastModifiedDate = DateTime.UtcNow;

                await _repository.UpdateAsync(comment);
                await _historyService.SaveHistory(comment, "SoftDelete");
                
                await _logService.CreateLog(
                    "Yorum Yumuşak Silme",
                    $"'{comment.Name}' tarafından yapılan yorum yumuşak silindi",
                    "SoftDelete",
                    "Comment"
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "CommentSoftDelete",
                    $"Yorum yumuşak silinirken hata: {request.Id}"
                );
                throw new AuFrameWorkException(
                    "Yorum yumuşak silinirken bir hata oluştu", 
                    "SOFT_DELETE_ERROR",
                    "Error"
                );
            }
        }
    }
} 