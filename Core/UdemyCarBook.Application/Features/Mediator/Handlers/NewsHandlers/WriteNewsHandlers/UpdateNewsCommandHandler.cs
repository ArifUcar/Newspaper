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
    public class UpdateNewsCommandHandler : IRequestHandler<UpdateNewsCommand, Unit>
    {
        private readonly INewsRepository _newsRepository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;
        private readonly IUserService _userService;
        private readonly IPermissionAuthorizationService _permissionAuthorizationService;
        private readonly IImageUploadService _imageUploadService;

        public UpdateNewsCommandHandler(
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

        public async Task<Unit> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userId = await _userService.GetCurrentUserIdAsync();
                if (!await _permissionAuthorizationService.HasPermissionAsync(userId, "NEWS_UPDATE"))
                {
                    await _logService.CreateLog(
                        "Yetki Hatası",
                        $"Kullanıcı ID: {userId}, İzin: NEWS_UPDATE, İşlem: Haber Güncelleme",
                        "Error",
                        "Authorization"
                    );
                    throw new AuFrameWorkException("Yetkiniz yok", "PERMISSION_DENIED", "Authorization");
                }

                var news = await _newsRepository.GetByIdAsync(request.Id);
                if (news == null)
                    throw new AuFrameWorkException("Haber bulunamadı", "NEWS_NOT_FOUND", "NotFound");

                if (!string.IsNullOrEmpty(request.CoverImageBase64))
                {
                    if (!string.IsNullOrEmpty(news.CoverImageUrl))
                    {
                        await _imageUploadService.DeleteImageAsync(news.CoverImageUrl);
                    }

                    var coverImageUrl = await _imageUploadService.UploadImageAsync(request.CoverImageBase64, "news");
                    if (coverImageUrl == null)
                    {
                        throw new AuFrameWorkException(
                            "Resim yüklenirken bir hata oluştu",
                            "IMAGE_UPLOAD_ERROR",
                            "Error"
                        );
                    }
                    news.CoverImageUrl = coverImageUrl;
                }

                news.Title = request.Title;
                news.Content = request.Content;
                news.Summary = request.Summary;
                news.Slug = request.Slug;
                news.IsFeatured = request.IsFeatured;
                news.IsActive = request.IsActive;
                news.IsPublished = request.IsPublished;
                news.PublishDate = request.PublishDate;
                news.MetaTitle = request.MetaTitle;
                news.MetaDescription = request.MetaDescription;
                news.MetaKeywords = request.MetaKeywords;
                news.CategoryId = request.CategoryId;
                news.UserId = request.UserId;
                news.LastModifiedDate = DateTime.UtcNow;
                news.LastModifiedByUserId = userId;

                await _newsRepository.UpdateAsync(news);
                await _historyService.SaveHistory(news, "Update");

                await _logService.CreateLog(
                    "Haber Güncellendi",
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
                    "UpdateNews",
                    "Haber güncellenirken hata oluştu"
                );
                throw new AuFrameWorkException(
                    "Haber güncellenirken bir hata oluştu",
                    "UPDATE_ERROR",
                    "Error"
                );
            }
        }
    }
} 