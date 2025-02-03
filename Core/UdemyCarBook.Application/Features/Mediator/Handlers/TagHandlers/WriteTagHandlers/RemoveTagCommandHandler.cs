using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Commands.TagCommands;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Application.Interfaces.IService;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Domain.Exceptions;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.TagHandlers.WriteTagHandlers
{
    public class RemoveTagCommandHandler : IRequestHandler<RemoveTagCommand>
    {
        private readonly ITagRepository _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public RemoveTagCommandHandler(ITagRepository repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task Handle(RemoveTagCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var tag = await _repository.GetByIdWithDetailsAsync(request.Id);
                if (tag == null)
                    throw new AuFrameWorkException("Etiket bulunamadı", "TAG_NOT_FOUND", "NotFound");

                if (tag.News != null && tag.News.Any())
                    throw new AuFrameWorkException("Bu etiket haberlerde kullanılıyor. Önce haberlerdeki etiketleri kaldırın.", "TAG_HAS_NEWS", "ValidationError");

                await _repository.RemoveAsync(tag);
                await _historyService.SaveHistory(tag, "Remove");
                
                await _logService.CreateLog(
                    "Etiket Silme",
                    $"'{tag.Name}' adlı etiket silindi",
                    "Remove",
                    "Tag"
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "TagRemove",
                    $"Etiket silinirken hata: {request.Id}"
                );
                throw new AuFrameWorkException(
                    "Etiket silinirken bir hata oluştu", 
                    "REMOVE_ERROR",
                    "Error"
                );
            }
        }
    }
} 