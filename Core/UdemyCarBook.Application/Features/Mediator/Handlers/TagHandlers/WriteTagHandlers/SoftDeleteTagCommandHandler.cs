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
    public class SoftDeleteTagCommandHandler : IRequestHandler<SoftDeleteTagCommand>
    {
        private readonly ITagRepository _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public SoftDeleteTagCommandHandler(ITagRepository repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task Handle(SoftDeleteTagCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var tag = await _repository.GetByIdWithDetailsAsync(request.Id);
                if (tag == null)
                    throw new AuFrameWorkException("Etiket bulunamadı", "TAG_NOT_FOUND", "NotFound");

                if (tag.News != null && tag.News.Any())
                    throw new AuFrameWorkException("Bu etiket haberlerde kullanılıyor. Önce haberlerdeki etiketleri kaldırın.", "TAG_HAS_NEWS", "ValidationError");

                tag.IsDeleted = true;
                tag.LastModifiedDate = DateTime.UtcNow;

                await _repository.UpdateAsync(tag);
                await _historyService.SaveHistory(tag, "SoftDelete");
                
                await _logService.CreateLog(
                    "Etiket Yumuşak Silme",
                    $"'{tag.Name}' adlı etiket yumuşak silindi",
                    "SoftDelete",
                    "Tag"
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "TagSoftDelete",
                    $"Etiket yumuşak silinirken hata: {request.Id}"
                );
                throw new AuFrameWorkException(
                    "Etiket yumuşak silinirken bir hata oluştu", 
                    "SOFT_DELETE_ERROR",
                    "Error"
                );
            }
        }
    }
} 