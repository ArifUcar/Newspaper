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
    public class UpdateSocialMediaCommandHandler : IRequestHandler<UpdateSocialMediaCommand>
    {
        private readonly ISocialMediaRepository _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public UpdateSocialMediaCommandHandler(ISocialMediaRepository repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task Handle(UpdateSocialMediaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var socialMedia = await _repository.GetByIdWithDetailsAsync(request.Id);
                if (socialMedia == null)
                    throw new AuFrameWorkException("Sosyal medya hesabı bulunamadı", "SOCIAL_MEDIA_NOT_FOUND", "NotFound");

                if (string.IsNullOrEmpty(request.Platform))
                    throw new AuFrameWorkException("Platform adı boş olamaz", "PLATFORM_REQUIRED", "ValidationError");

                if (string.IsNullOrEmpty(request.Url))
                    throw new AuFrameWorkException("URL boş olamaz", "URL_REQUIRED", "ValidationError");

                if (string.IsNullOrEmpty(request.Icon))
                    throw new AuFrameWorkException("İkon boş olamaz", "ICON_REQUIRED", "ValidationError");

                if (socialMedia.Platform != request.Platform || socialMedia.UserId != request.UserId)
                {
                    var isPlatformExists = await _repository.IsPlatformExistsForAuthorAsync(request.Platform, request.UserId);
                    if (isPlatformExists)
                        throw new AuFrameWorkException("Bu yazar için bu platform zaten eklenmiş", "PLATFORM_EXISTS", "ValidationError");
                }

                socialMedia.Platform = request.Platform;
                socialMedia.Url = request.Url;
                socialMedia.Icon = request.Icon;
                socialMedia.DisplayOrder = request.DisplayOrder;
                socialMedia.IsActive = request.IsActive;
                socialMedia.FollowerCount = request.FollowerCount;
                socialMedia.AccountName = request.AccountName;
                socialMedia.UserId = request.UserId;
                socialMedia.LastModifiedDate = DateTime.UtcNow;

                await _repository.UpdateAsync(socialMedia);
                await _historyService.SaveHistory(socialMedia, "Update");
                
                await _logService.CreateLog(
                    "Sosyal Medya Hesabı Güncelleme",
                    $"'{request.Platform}' platformu için sosyal medya hesabı güncellendi",
                    "Update",
                    "SocialMedia"
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "SocialMediaUpdate",
                    $"Sosyal medya hesabı güncellenirken hata: {request.Platform}"
                );
                throw new AuFrameWorkException(
                    "Sosyal medya hesabı güncellenirken bir hata oluştu", 
                    "UPDATE_ERROR",
                    "Error"
                );
            }
        }
    }
} 