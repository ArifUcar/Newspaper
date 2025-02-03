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
    public class CreateSocialMediaCommandHandler : IRequestHandler<CreateSocialMediaCommand>
    {
        private readonly ISocialMediaRepository _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public CreateSocialMediaCommandHandler(ISocialMediaRepository repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task Handle(CreateSocialMediaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Platform))
                    throw new AuFrameWorkException("Platform adı boş olamaz", "PLATFORM_REQUIRED", "ValidationError");

                if (string.IsNullOrEmpty(request.Url))
                    throw new AuFrameWorkException("URL boş olamaz", "URL_REQUIRED", "ValidationError");

                if (string.IsNullOrEmpty(request.Icon))
                    throw new AuFrameWorkException("İkon boş olamaz", "ICON_REQUIRED", "ValidationError");

                var isPlatformExists = await _repository.IsPlatformExistsForAuthorAsync(request.Platform, request.UserId);
                if (isPlatformExists)
                    throw new AuFrameWorkException("Bu yazar için bu platform zaten eklenmiş", "PLATFORM_EXISTS", "ValidationError");

                var socialMedia = new SocialMedia
                {
                    Id = Guid.NewGuid(),
                    Platform = request.Platform,
                    Url = request.Url,
                    Icon = request.Icon,
                    DisplayOrder = request.DisplayOrder,
                    IsActive = request.IsActive,
                    FollowerCount = request.FollowerCount,
                    AccountName = request.AccountName,
                    UserId = request.UserId,
                    IsDeleted = false
                };

                await _repository.CreateAsync(socialMedia);
                await _historyService.SaveHistory(socialMedia, "Create");
                
                await _logService.CreateLog(
                    "Sosyal Medya Hesabı Oluşturma",
                    $"'{request.Platform}' platformu için sosyal medya hesabı oluşturuldu",
                    "Create",
                    "SocialMedia"
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "SocialMediaCreate",
                    $"Sosyal medya hesabı oluşturulurken hata: {request.Platform}"
                );
                throw new AuFrameWorkException(
                    "Sosyal medya hesabı oluşturulurken bir hata oluştu", 
                    "CREATE_ERROR",
                    "Error"
                );
            }
        }
    }
} 