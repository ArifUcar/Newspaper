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
    public class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand>
    {
        private readonly ITagRepository _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public UpdateTagCommandHandler(ITagRepository repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var tag = await _repository.GetByIdWithDetailsAsync(request.Id);
                if (tag == null)
                    throw new AuFrameWorkException("Etiket bulunamadı", "TAG_NOT_FOUND", "NotFound");

                if (tag.Name != request.Name && await _repository.IsNameExistsAsync(request.Name))
                    throw new AuFrameWorkException("Bu etiket adı zaten kullanılıyor", "NAME_EXISTS", "ValidationError");

                tag.Name = request.Name;
              
                tag.LastModifiedDate = DateTime.UtcNow;

                await _repository.UpdateAsync(tag);
                await _historyService.SaveHistory(tag, "Update");
                
                await _logService.CreateLog(
                    "Etiket Güncelleme",
                    $"'{tag.Name}' adlı etiket güncellendi",
                    "Update",
                    "Tag"
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "TagUpdate",
                    $"Etiket güncellenirken hata: {request.Id}"
                );
                throw new AuFrameWorkException(
                    "Etiket güncellenirken bir hata oluştu", 
                    "UPDATE_ERROR",
                    "Error"
                );
            }
        }
    }
} 