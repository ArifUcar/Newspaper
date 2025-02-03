using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Commands.AboutCommands;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Application.Interfaces.IService;
using UdemyCarBook.Domain.Entities;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.AboutHandlers.WriteAboutHandlers
{
    public class SoftDeleteAboutCommandHandler : IRequestHandler<SoftDeleteAboutCommand>
    {
        private readonly IRepository<About> _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public SoftDeleteAboutCommandHandler(IRepository<About> repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task Handle(SoftDeleteAboutCommand request, CancellationToken cancellationToken)
        {
            var value = await _repository.GetByIdAsync(request.Id);
            if (value != null)
            {
                // Soft delete öncesi history
                await _historyService.SaveHistory(value, "BeforeSoftDelete");

                await _repository.SoftDeleteAsync(request.Id);

                value.IsDeleted = true;
                // Soft delete sonrası history
                await _historyService.SaveHistory(value, "AfterSoftDelete");

                // Log kaydı
                await _logService.CreateLog(
                    "About Soft Delete",
                    $"ID: {request.Id}, Başlık: {value.Title} olan about soft delete edildi",
                    "SoftDelete",
                    "About"
                );
            }
        }
    }
}
