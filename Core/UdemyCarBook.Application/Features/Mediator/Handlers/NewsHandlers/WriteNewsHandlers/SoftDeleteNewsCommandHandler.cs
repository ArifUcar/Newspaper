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
    public class SoftDeleteNewsCommandHandler : IRequestHandler<SoftDeleteNewsCommand, Unit>
    {
        private readonly INewsRepository _newsRepository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;
        private readonly IUserService _userService;
        private readonly IPermissionAuthorizationService _permissionAuthorizationService;
        private readonly IImageUploadService _imageUploadService;

        public SoftDeleteNewsCommandHandler(
            INewsRepository newsRepository,
            IHistoryService historyService,
            ILogService logService,
            IUserService userService,
            IPermissionAuthorizationService permissionAuthorizationService,
            IImageUploadService imageUploadService)
        {
            _newsRepository = newsRepository;
            _historyService = historyService;
            _logService = logService;
            _userService = userService;
            _permissionAuthorizationService = permissionAuthorizationService;
            _imageUploadService = imageUploadService;
        }

        public async Task<Unit> Handle(SoftDeleteNewsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userId = await _userService.GetCurrentUserIdAsync();
                if (!await _permissionAuthorizationService.HasPermissionAsync(userId, "NEWS_DELETE"))
                {
                    await _logService.CreateLog(
                        "Yetki Hatası",
                        $"Kullanıcı ID: {userId}, İzin: NEWS_DELETE, İşlem: Haber Silme",
                        "Error",
                        "Authorization"
                    );
                    throw new AuFrameWorkException("Yetkiniz yok", "PERMISSION_DENIED", "Authorization");
                }

                var news = await _newsRepository.GetByIdAsync(request.Id);
                if (news == null)
                    throw new AuFrameWorkException("Haber bulunamadı", "NEWS_NOT_FOUND", "NotFound");

                if (!string.IsNullOrEmpty(news.CoverImageUrl))
                {
                    await _imageUploadService.DeleteImageAsync(news.CoverImageUrl);
                }

                news.IsDeleted = true;
                news.LastModifiedDate = DateTime.UtcNow;
                news.LastModifiedByUserId = userId;

                await _newsRepository.UpdateAsync(news);
                await _historyService.SaveHistory(news, "SoftDelete");

                await _logService.CreateLog(
                    "Haber Silindi",
                    $"Kullanıcı ID: {userId}, Haber ID: {request.Id}",
                    "Information",
                    "News"
                );

                return Unit.Value;
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "DeleteNews",
                    "Haber silinirken hata oluştu"
                );
                throw new AuFrameWorkException(
                    "Haber silinirken bir hata oluştu",
                    "DELETE_ERROR",
                    "Error"
                );
            }
        }
    }
} 