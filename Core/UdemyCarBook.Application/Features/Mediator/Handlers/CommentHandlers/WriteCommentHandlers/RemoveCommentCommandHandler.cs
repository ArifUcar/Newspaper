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
    public class RemoveCommentCommandHandler : IRequestHandler<RemoveCommentCommand>
    {
        private readonly IRepository<Comment> _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public RemoveCommentCommandHandler(IRepository<Comment> repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task Handle(RemoveCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var comment = await _repository.GetByIdAsync(request.Id);
                if (comment == null)
                    throw new AuFrameWorkException("Yorum bulunamadı", "COMMENT_NOT_FOUND", "NotFound");

                await _repository.RemoveAsync(comment);
                await _historyService.SaveHistory(comment, "Remove");
                
                await _logService.CreateLog(
                    "Yorum Silme",
                    $"'{comment.Name}' tarafından yapılan yorum silindi",
                    "Remove",
                    "Comment"
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "CommentRemove",
                    $"Yorum silinirken hata: {request.Id}"
                );
                throw new AuFrameWorkException(
                    "Yorum silinirken bir hata oluştu", 
                    "REMOVE_ERROR",
                    "Error"
                );
            }
        }
    }
} 