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
    public class RemoveContactCommandHandler : IRequestHandler<RemoveContactCommand>
    {
        private readonly IRepository<Contact> _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public RemoveContactCommandHandler(IRepository<Contact> repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task Handle(RemoveContactCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var contact = await _repository.GetByIdAsync(request.Id);
                if (contact == null)
                    throw new AuFrameWorkException("İletişim mesajı bulunamadı", "CONTACT_NOT_FOUND", "NotFound");

                await _repository.RemoveAsync(contact);
                await _historyService.SaveHistory(contact, "Remove");
                
                await _logService.CreateLog(
                    "İletişim Mesajı Silme",
                    $"'{contact.Name}' tarafından gönderilen iletişim mesajı silindi",
                    "Remove",
                    "Contact"
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "ContactRemove",
                    $"İletişim mesajı silinirken hata: {request.Id}"
                );
                throw new AuFrameWorkException(
                    "İletişim mesajı silinirken bir hata oluştu", 
                    "REMOVE_ERROR",
                    "Error"
                );
            }
        }
    }
} 