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
    public class SoftDeleteContactCommandHandler : IRequestHandler<SoftDeleteContactCommand>
    {
        private readonly IRepository<Contact> _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public SoftDeleteContactCommandHandler(IRepository<Contact> repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task Handle(SoftDeleteContactCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var contact = await _repository.GetByIdAsync(request.Id);
                if (contact == null)
                    throw new AuFrameWorkException("İletişim mesajı bulunamadı", "CONTACT_NOT_FOUND", "NotFound");

                contact.IsDeleted = true;
                contact.LastModifiedDate = DateTime.UtcNow;

                await _repository.UpdateAsync(contact);
                await _historyService.SaveHistory(contact, "SoftDelete");
                
                await _logService.CreateLog(
                    "İletişim Mesajı Yumuşak Silme",
                    $"'{contact.Name}' tarafından gönderilen iletişim mesajı yumuşak silindi",
                    "SoftDelete",
                    "Contact"
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "ContactSoftDelete",
                    $"İletişim mesajı yumuşak silinirken hata: {request.Id}"
                );
                throw new AuFrameWorkException(
                    "İletişim mesajı yumuşak silinirken bir hata oluştu", 
                    "SOFT_DELETE_ERROR",
                    "Error"
                );
            }
        }
    }
} 