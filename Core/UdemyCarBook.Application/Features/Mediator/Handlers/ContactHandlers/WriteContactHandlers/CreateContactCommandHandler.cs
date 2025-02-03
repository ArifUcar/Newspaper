using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Commands.ContactCommands;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Application.Interfaces.IService;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Domain.Exceptions;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.ContactHandlers.WriteContactHandlers
{
    public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand>
    {
        private readonly IRepository<Contact> _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public CreateContactCommandHandler(IRepository<Contact> repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task Handle(CreateContactCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Name))
                    throw new AuFrameWorkException("Ad boş olamaz", "NAME_REQUIRED", "ValidationError");

                if (string.IsNullOrEmpty(request.Email))
                    throw new AuFrameWorkException("E-posta adresi boş olamaz", "EMAIL_REQUIRED", "ValidationError");

                if (string.IsNullOrEmpty(request.Subject))
                    throw new AuFrameWorkException("Konu boş olamaz", "SUBJECT_REQUIRED", "ValidationError");

                if (string.IsNullOrEmpty(request.Message))
                    throw new AuFrameWorkException("Mesaj boş olamaz", "MESSAGE_REQUIRED", "ValidationError");

                var contact = new Contact
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Email = request.Email,
                    Subject = request.Subject,
                    Message = request.Message,
                    
                    IsDeleted = false
                };

                await _repository.CreateAsync(contact);
                await _historyService.SaveHistory(contact, "Create");
                
                await _logService.CreateLog(
                    "İletişim Mesajı Oluşturma",
                    $"'{request.Name}' tarafından iletişim mesajı oluşturuldu",
                    "Create",
                    "Contact"
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "ContactCreate",
                    $"İletişim mesajı oluşturulurken hata: {request.Name}"
                );
                throw new AuFrameWorkException(
                    "İletişim mesajı oluşturulurken bir hata oluştu", 
                    "CREATE_ERROR",
                    "Error"
                );
            }
        }
    }
} 