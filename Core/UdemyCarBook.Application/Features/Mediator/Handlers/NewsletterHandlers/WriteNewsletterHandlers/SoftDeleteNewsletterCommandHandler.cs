using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Commands.NewsletterCommands;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Application.Interfaces.IService;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Domain.Exceptions;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.NewsletterHandlers.WriteNewsletterHandlers
{
    public class SoftDeleteNewsletterCommandHandler : IRequestHandler<SoftDeleteNewsletterCommand>
    {
        private readonly INewsletterRepository _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public SoftDeleteNewsletterCommandHandler(INewsletterRepository repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task Handle(SoftDeleteNewsletterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newsletter = await _repository.GetByIdWithDetailsAsync(request.Id);
                if (newsletter == null)
                    throw new AuFrameWorkException("Bülten aboneliği bulunamadı", "NEWSLETTER_NOT_FOUND", "NotFound");

                newsletter.IsDeleted = true;
                newsletter.LastModifiedDate = DateTime.UtcNow;

                await _repository.UpdateAsync(newsletter);
                await _historyService.SaveHistory(newsletter, "SoftDelete");
                
                await _logService.CreateLog(
                    "Bülten Aboneliği Yumuşak Silme",
                    $"'{newsletter.Email}' e-posta adresi bülten aboneliği yumuşak silindi",
                    "SoftDelete",
                    "Newsletter"
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "NewsletterSoftDelete",
                    $"Bülten aboneliği yumuşak silinirken hata: {request.Id}"
                );
                throw new AuFrameWorkException(
                    "Bülten aboneliği yumuşak silinirken bir hata oluştu", 
                    "SOFT_DELETE_ERROR",
                    "Error"
                );
            }
        }
    }
} 