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
    public class SoftDeleteSocialMediaCommandHandler : IRequestHandler<SoftDeleteSocialMediaCommand>
    {
        private readonly ISocialMediaRepository _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public SoftDeleteSocialMediaCommandHandler(ISocialMediaRepository repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task Handle(SoftDeleteSocialMediaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var socialMedia = await _repository.GetByIdWithDetailsAsync(request.Id);
                if (socialMedia == null)
                    throw new AuFrameWorkException("Sosyal medya hesabı bulunamadı", "SOCIAL_MEDIA_NOT_FOUND", "NotFound");

                socialMedia.IsDeleted = true;
                socialMedia.LastModifiedDate = DateTime.UtcNow;

                await _repository.UpdateAsync(socialMedia);
                await _historyService.SaveHistory(socialMedia, "SoftDelete");
                
                await _logService.CreateLog(
                    "Sosyal Medya Hesabı Yumuşak Silme",
                    $"'{socialMedia.Platform}' platformu için sosyal medya hesabı yumuşak silindi",
                    "SoftDelete",
                    "SocialMedia"
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "SocialMediaSoftDelete",
                    $"Sosyal medya hesabı yumuşak silinirken hata: {request.Id}"
                );
                throw new AuFrameWorkException(
                    "Sosyal medya hesabı yumuşak silinirken bir hata oluştu", 
                    "SOFT_DELETE_ERROR",
                    "Error"
                );
            }
        }
    }
} 