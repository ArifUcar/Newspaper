using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Commands.NewsCommands;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Application.Interfaces.IService;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Domain.Exceptions;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.NewsHandlers.WriteNewsHandlers
{
    public class RemoveNewsCommandHandler : IRequestHandler<RemoveNewsCommand>
    {
        private readonly IRepository<News> _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public RemoveNewsCommandHandler(IRepository<News> repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task Handle(RemoveNewsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var news = await _repository.GetByIdAsync(request.Id);
                if (news == null)
                    throw new AuFrameWorkException("Haber bulunamadı", "NEWS_NOT_FOUND", "NotFound");

                await _repository.RemoveAsync(news);
                await _historyService.SaveHistory(news, "Remove");
                
                await _logService.CreateLog(
                    "Haber Silme",
                    $"'{news.Title}' başlıklı haber silindi",
                    "Remove",
                    "News"
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "NewsRemove",
                    $"Haber silinirken hata: {request.Id}"
                );
                throw new AuFrameWorkException(
                    "Haber silinirken bir hata oluştu", 
                    "REMOVE_ERROR",
                    "Error"
                );
            }
        }
    }
} 