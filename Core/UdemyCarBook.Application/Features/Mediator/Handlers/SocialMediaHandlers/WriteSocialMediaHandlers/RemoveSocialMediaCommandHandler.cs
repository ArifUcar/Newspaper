using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Commands.SocialMediaCommands;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Application.Interfaces.IService;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Domain.Exceptions;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.SocialMediaHandlers.WriteSocialMediaHandlers
{
    public class RemoveSocialMediaCommandHandler : IRequestHandler<RemoveSocialMediaCommand>
    {
        private readonly ISocialMediaRepository _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public RemoveSocialMediaCommandHandler(ISocialMediaRepository repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task Handle(RemoveSocialMediaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var socialMedia = await _repository.GetByIdWithDetailsAsync(request.Id);
                if (socialMedia == null)
                    throw new AuFrameWorkException("Sosyal medya hesabı bulunamadı", "SOCIAL_MEDIA_NOT_FOUND", "NotFound");

                await _repository.RemoveAsync(socialMedia);
                await _historyService.SaveHistory(socialMedia, "Remove");
                
                await _logService.CreateLog(
                    "Sosyal Medya Hesabı Silme",
                    $"'{socialMedia.Platform}' platformu için sosyal medya hesabı silindi",
                    "Remove",
                    "SocialMedia"
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "SocialMediaRemove",
                    $"Sosyal medya hesabı silinirken hata: {request.Id}"
                );
                throw new AuFrameWorkException(
                    "Sosyal medya hesabı silinirken bir hata oluştu", 
                    "REMOVE_ERROR",
                    "Error"
                );
            }
        }
    }
} 