using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Queries.ContactQueries;
using UdemyCarBook.Application.Features.Mediator.Results.ContactResults;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Domain.Entities;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.ContactHandlers.ReadContactHandlers
{
    public class GetContactQueryHandler : IRequestHandler<GetContactQuery, List<GetContactQueryResult>>
    {
        private readonly IContactRepository _repository;

        public GetContactQueryHandler(IContactRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GetContactQueryResult>> Handle(GetContactQuery request, CancellationToken cancellationToken)
        {
            var contacts = await _repository.GetAllWithDetailsAsync();

            return contacts.Select(x => new GetContactQueryResult
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                Subject = x.Subject,
                Message = x.Message,
                IsRead = x.IsRead,
                IsReplied = x.IsReplied,
                ReplyDate = x.ReplyDate,
                ReplyMessage = x.ReplyMessage,
                CreatedDate = x.CreatedDate,
           
            }).ToList();
        }
    }
} 