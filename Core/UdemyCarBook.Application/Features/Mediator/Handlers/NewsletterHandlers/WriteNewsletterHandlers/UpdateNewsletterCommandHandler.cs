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
    public class UpdateNewsletterCommandHandler : IRequestHandler<UpdateNewsletterCommand>
    {
        private readonly INewsletterRepository _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public UpdateNewsletterCommandHandler(INewsletterRepository repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task Handle(UpdateNewsletterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newsletter = await _repository.GetByIdWithDetailsAsync(request.Id);
                if (newsletter == null)
                    throw new AuFrameWorkException("Bülten aboneliği bulunamadı", "NEWSLETTER_NOT_FOUND", "NotFound");

                if (string.IsNullOrEmpty(request.Email))
                    throw new AuFrameWorkException("E-posta adresi boş olamaz", "EMAIL_REQUIRED", "ValidationError");

                if (newsletter.Email != request.Email)
                {
                    var isEmailSubscribed = await _repository.IsEmailSubscribedAsync(request.Email);
                    if (isEmailSubscribed)
                        throw new AuFrameWorkException("Bu e-posta adresi zaten kayıtlı", "EMAIL_ALREADY_SUBSCRIBED", "ValidationError");
                }

                newsletter.Email = request.Email;
                newsletter.IsActive = request.IsActive;
                newsletter.UnsubscribeDate = !request.IsActive ? DateTime.UtcNow : null;
                newsletter.LastModifiedDate = DateTime.UtcNow;

                await _repository.UpdateAsync(newsletter);
                await _historyService.SaveHistory(newsletter, "Update");
                
                await _logService.CreateLog(
                    "Bülten Aboneliği Güncelleme",
                    $"'{request.Email}' e-posta adresi bülten aboneliği güncellendi",
                    "Update",
                    "Newsletter"
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "NewsletterUpdate",
                    $"Bülten aboneliği güncellenirken hata: {request.Email}"
                );
                throw new AuFrameWorkException(
                    "Bülten aboneliği güncellenirken bir hata oluştu", 
                    "UPDATE_ERROR",
                    "Error"
                );
            }
        }
    }
} 