using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Commands.AboutCommands;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Application.Interfaces.IService;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Domain.Exceptions;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.AboutHandlers.WriteAboutHandlers
{
    public class RemoveAboutCommandHandler : IRequestHandler<RemoveAboutCommand>
    {
        private readonly IRepository<About> _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public RemoveAboutCommandHandler(IRepository<About> repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task Handle(RemoveAboutCommand request, CancellationToken cancellationToken)
        {
            var value = await _repository.GetByIdAsync(request.Id);
            
            if (value == null)
            {
                // Log kaydı
                await _logService.CreateLog(
                    "About Silme Hatası",
                    $"ID: {request.Id} olan about bulunamadı",
                    "Error",
                    "About"
                );

                throw new AuFrameWorkException(
                    $"ID: {request.Id} olan about kaydı bulunamadı.", 
                    "RECORD_NOT_FOUND",
                    "NotFound"
                );
            }

            try
            {
                // Silme öncesi history
                await _historyService.SaveHistory(value, "BeforeDelete");

                var title = value.Title;
                await _repository.RemoveAsync(value);

                // Silme sonrası history
                var deletedRecord = new { Id = request.Id, Title = title, DeletedAt = DateTime.Now };
                await _historyService.SaveHistory(deletedRecord, "AfterDelete");

                // Başarılı silme log kaydı
                await _logService.CreateLog(
                    "About Silme",
                    $"ID: {request.Id}, Başlık: {title} olan about başarıyla silindi",
                    "Delete",
                    "About"
                );
            }
            catch (Exception ex)
            {
                // Hata durumunda log
                await _logService.CreateErrorLog(
                    ex,
                    "AboutDelete",
                    $"ID: {request.Id} olan about silinirken hata oluştu"
                );

                throw new AuFrameWorkException(
                    "About silinirken bir hata oluştu.", 
                    "DELETE_ERROR",
                    "Error"
                );
            }
        }
    }
}
