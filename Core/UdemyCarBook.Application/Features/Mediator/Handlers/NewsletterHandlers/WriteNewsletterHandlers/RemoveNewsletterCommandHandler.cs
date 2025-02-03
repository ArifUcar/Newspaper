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
    public class RemoveNewsletterCommandHandler : IRequestHandler<RemoveNewsletterCommand>
    {
        private readonly INewsletterRepository _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public RemoveNewsletterCommandHandler(INewsletterRepository repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task Handle(RemoveNewsletterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newsletter = await _repository.GetByIdWithDetailsAsync(request.Id);
                if (newsletter == null)
                    throw new AuFrameWorkException("Bülten aboneliği bulunamadı", "NEWSLETTER_NOT_FOUND", "NotFound");

                await _repository.RemoveAsync(newsletter);
                await _historyService.SaveHistory(newsletter, "Remove");
                
                await _logService.CreateLog(
                    "Bülten Aboneliği Silme",
                    $"'{newsletter.Email}' e-posta adresi bülten aboneliği silindi",
                    "Remove",
                    "Newsletter"
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "NewsletterRemove",
                    $"Bülten aboneliği silinirken hata: {request.Id}"
                );
                throw new AuFrameWorkException(
                    "Bülten aboneliği silinirken bir hata oluştu", 
                    "REMOVE_ERROR",
                    "Error"
                );
            }
        }
    }
} 