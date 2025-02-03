using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Commands.TagCommands;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Application.Interfaces.IService;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Domain.Exceptions;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.TagHandlers.WriteTagHandlers
{
    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand>
    {
        private readonly ITagRepository _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public CreateTagCommandHandler(ITagRepository repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (await _repository.IsNameExistsAsync(request.Name))
                    throw new AuFrameWorkException("Bu etiket adı zaten kullanılıyor", "NAME_EXISTS", "ValidationError");

                var tag = new Tag
                {
                    Name = request.Name,
            
                    CreatedDate = DateTime.UtcNow
                };

                await _repository.CreateAsync(tag);
                await _historyService.SaveHistory(tag, "Create");
                
                await _logService.CreateLog(
                    "Etiket Oluşturma",
                    $"'{tag.Name}' adlı etiket oluşturuldu",
                    "Create",
                    "Tag"
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "TagCreate",
                    $"Etiket oluşturulurken hata: {request.Name}"
                );
                throw new AuFrameWorkException(
                    "Etiket oluşturulurken bir hata oluştu", 
                    "CREATE_ERROR",
                    "Error"
                );
            }
        }
    }
} 