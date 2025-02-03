using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Queries.ContactQueries;
using UdemyCarBook.Application.Features.Mediator.Results.ContactResults;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Domain.Exceptions;
using System.Linq;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.ContactHandlers.ReadContactHandlers
{
    public class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQuery, GetContactByIdQueryResult>
    {
        private readonly IContactRepository _repository;

        public GetContactByIdQueryHandler(IContactRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetContactByIdQueryResult> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
        {
            var contact = await _repository.GetByIdWithDetailsAsync(request.Id);

            if (contact == null)
                throw new AuFrameWorkException("İletişim mesajı bulunamadı", "CONTACT_NOT_FOUND", "NotFound");

            return new GetContactByIdQueryResult
            {
                Id = contact.Id,
                Name = contact.Name,
                Email = contact.Email,
                Subject = contact.Subject,
                Message = contact.Message,
                IsRead = contact.IsRead,
                IsReplied = contact.IsReplied,
                ReplyDate = contact.ReplyDate,
                ReplyMessage = contact.ReplyMessage,
                CreatedDate = contact.CreatedDate,
                CreatedByUserName = contact.CreatedByUser != null ? contact.CreatedByUser.UserName : null,
                LastModifiedDate = contact.LastModifiedDate,
                LastModifiedByUserName = contact.LastModifiedByUser != null ? contact.LastModifiedByUser.UserName : null
            };
        }
    }
} 