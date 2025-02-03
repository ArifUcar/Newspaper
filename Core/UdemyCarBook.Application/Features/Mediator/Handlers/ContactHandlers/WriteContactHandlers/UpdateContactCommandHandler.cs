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
    public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand>
    {
        private readonly IRepository<Contact> _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public UpdateContactCommandHandler(IRepository<Contact> repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task Handle(UpdateContactCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var contact = await _repository.GetByIdAsync(request.Id);
                if (contact == null)
                    throw new AuFrameWorkException("İletişim mesajı bulunamadı", "CONTACT_NOT_FOUND", "NotFound");

                if (string.IsNullOrEmpty(request.Name))
                    throw new AuFrameWorkException("Ad boş olamaz", "NAME_REQUIRED", "ValidationError");

                if (string.IsNullOrEmpty(request.Email))
                    throw new AuFrameWorkException("E-posta adresi boş olamaz", "EMAIL_REQUIRED", "ValidationError");

                if (string.IsNullOrEmpty(request.Subject))
                    throw new AuFrameWorkException("Konu boş olamaz", "SUBJECT_REQUIRED", "ValidationError");

                if (string.IsNullOrEmpty(request.Message))
                    throw new AuFrameWorkException("Mesaj boş olamaz", "MESSAGE_REQUIRED", "ValidationError");

                contact.Name = request.Name;
                contact.Email = request.Email;
                contact.Subject = request.Subject;
                contact.Message = request.Message;
                contact.IsRead = request.IsRead;
                contact.IsReplied = request.IsReplied;
                contact.ReplyDate = request.ReplyDate;
                contact.ReplyMessage = request.ReplyMessage;
                contact.LastModifiedDate = DateTime.UtcNow;

                await _repository.UpdateAsync(contact);
                await _historyService.SaveHistory(contact, "Update");
                
                await _logService.CreateLog(
                    "İletişim Mesajı Güncelleme",
                    $"'{request.Name}' tarafından gönderilen iletişim mesajı güncellendi",
                    "Update",
                    "Contact"
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "ContactUpdate",
                    $"İletişim mesajı güncellenirken hata: {request.Name}"
                );
                throw new AuFrameWorkException(
                    "İletişim mesajı güncellenirken bir hata oluştu", 
                    "UPDATE_ERROR",
                    "Error"
                );
            }
        }
    }
} 