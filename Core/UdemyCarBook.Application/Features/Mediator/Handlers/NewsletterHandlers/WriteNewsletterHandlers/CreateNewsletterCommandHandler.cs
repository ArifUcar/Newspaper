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
    public class CreateNewsletterCommandHandler : IRequestHandler<CreateNewsletterCommand>
    {
        private readonly INewsletterRepository _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public CreateNewsletterCommandHandler(INewsletterRepository repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task Handle(CreateNewsletterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email))
                    throw new AuFrameWorkException("E-posta adresi boş olamaz", "EMAIL_REQUIRED", "ValidationError");

                var isEmailSubscribed = await _repository.IsEmailSubscribedAsync(request.Email);
                if (isEmailSubscribed)
                    throw new AuFrameWorkException("Bu e-posta adresi zaten kayıtlı", "EMAIL_ALREADY_SUBSCRIBED", "ValidationError");

                var newsletter = new Newsletter
                {
                    Id = Guid.NewGuid(),
                    Email = request.Email,
                    IsActive = request.IsActive,
                    SubscriptionDate = DateTime.UtcNow,
                    IsDeleted = false
                };

                await _repository.CreateAsync(newsletter);
                await _historyService.SaveHistory(newsletter, "Create");
                
                await _logService.CreateLog(
                    "Bülten Aboneliği Oluşturma",
                    $"'{request.Email}' e-posta adresi bülten aboneliği oluşturuldu",
                    "Create",
                    "Newsletter"
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "NewsletterCreate",
                    $"Bülten aboneliği oluşturulurken hata: {request.Email}"
                );
                throw new AuFrameWorkException(
                    "Bülten aboneliği oluşturulurken bir hata oluştu", 
                    "CREATE_ERROR",
                    "Error"
                );
            }
        }
    }
} 