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
    public class CreateNewsCommandHandler : IRequestHandler<CreateNewsCommand, Unit>
    {
        private readonly INewsRepository _newsRepository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;
        private readonly IUserService _userService;
        private readonly IPermissionAuthorizationService _permissionAuthorizationService;
        private readonly IImageUploadService _imageUploadService;
        private readonly IRepository<Category> _categoryRepository;

        public CreateNewsCommandHandler(
            INewsRepository newsRepository,
            IHistoryService historyService,
            ILogService logService,
            IUserService userService,
            IPermissionAuthorizationService permissionAuthorizationService,
            IImageUploadService imageUploadService,
            IRepository<Category> categoryRepository)
        {
            _newsRepository = newsRepository;
            _historyService = historyService;
            _logService = logService;
            _userService = userService;
            _permissionAuthorizationService = permissionAuthorizationService;
            _imageUploadService = imageUploadService;
            _categoryRepository = categoryRepository;
        }

        public async Task<Unit> Handle(CreateNewsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userId = await _userService.GetCurrentUserIdAsync();
                if (!await _permissionAuthorizationService.HasPermissionAsync(userId, "NEWS_CREATE"))
                {
                    await _logService.CreateLog(
                        "Yetki Hatası",
                        $"Kullanıcı ID: {userId}, İzin: NEWS_CREATE, İşlem: Haber Oluşturma",
                        "Error",
                        "Authorization"
                    );
                    throw new AuFrameWorkException("Yetkiniz yok", "PERMISSION_DENIED", "Authorization");
                }

                // Kategori kontrolü
                var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
                if (category == null)
                {
                    throw new AuFrameWorkException(
                        "Belirtilen kategori bulunamadı",
                        "CATEGORY_NOT_FOUND",
                        "NotFound"
                    );
                }

                string coverImageUrl = null;
                if (!string.IsNullOrEmpty(request.CoverImageBase64))
                {
                    coverImageUrl = await _imageUploadService.UploadImageAsync(request.CoverImageBase64, "news");
                    if (coverImageUrl == null)
                    {
                        throw new AuFrameWorkException(
                            "Resim yüklenirken bir hata oluştu",
                            "IMAGE_UPLOAD_ERROR",
                            "Error"
                        );
                    }
                }

                var news = new News
                {
                    Title = request.Title,
                    Content = request.Content,
                    Summary = request.Summary,
                    Slug = request.Slug,
                    CoverImageUrl = coverImageUrl,
                    IsFeatured = request.IsFeatured,
                    IsActive = request.IsActive,
                    IsPublished = request.IsPublished,
                    PublishDate = request.PublishDate,
                    MetaTitle = request.MetaTitle,
                    MetaDescription = request.MetaDescription,
                    MetaKeywords = request.MetaKeywords,
                    CategoryId = request.CategoryId,
                    UserId = userId,
                    CreatedDate = DateTime.UtcNow,
                    CreatedById = userId,
                    LastModifiedDate = DateTime.UtcNow,
                    LastModifiedByUserId = userId
                };

                await _newsRepository.CreateAsync(news);
                await _historyService.SaveHistory(news, "Create");

                await _logService.CreateLog(
                    "Haber Oluşturuldu",
                    $"Kullanıcı ID: {userId}, Başlık: {request.Title}",
                    "Information",
                    "News"
                );

                return Unit.Value;
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "CreateNews",
                    "Haber oluşturulurken hata oluştu"
                );
                throw new AuFrameWorkException(
                    "Haber oluşturulurken bir hata oluştu",
                    "CREATE_ERROR",
                    "Error"
                );
            }
        }
    }
} 