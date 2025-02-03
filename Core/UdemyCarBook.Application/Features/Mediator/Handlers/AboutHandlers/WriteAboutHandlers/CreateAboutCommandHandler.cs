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
using UdemyCarBook.Domain.Exceptions;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.AboutHandlers.WriteAboutHandlers
{
    public class CreateAboutCommandHandler : IRequestHandler<CreateAboutCommand>
    {
        private readonly IRepository<About> _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public CreateAboutCommandHandler(IRepository<About> repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task Handle(CreateAboutCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Title))
                    throw new AuFrameWorkException("Başlık boş olamaz", "TITLE_REQUIRED", "ValidationError");

                var about = new About
                {
                    Id = Guid.NewGuid(),
                    Title = request.Title,
                    Description = request.Description,
                    ImageUrl = request.ImageUrl,
                    IsDeleted = false
                };

                await _repository.CreateAsync(about);
                await _historyService.SaveHistory(about, "Create");
                
                await _logService.CreateLog(
                    "About Oluşturma",
                    $"'{request.Title}' başlıklı about oluşturuldu",
                    "Create",
                    "About"
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "AboutCreate",
                    $"About oluşturulurken hata: {request.Title}"
                );
                throw new AuFrameWorkException(
                    "About oluşturulurken bir hata oluştu", 
                    "CREATE_ERROR",
                    "Error"
                );
            }
        }
    }
}
