using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Commands.CategoryCommands;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Application.Interfaces.IService;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Domain.Exceptions;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.CategoryHandlers.WriteCategoryHandlers
{
    public class SoftDeleteCategoryCommandHandler : IRequestHandler<SoftDeleteCategoryCommand, Unit>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;
        private readonly IUserService _userService;
        private readonly IPermissionAuthorizationService _permissionAuthorizationService;

        public SoftDeleteCategoryCommandHandler(
            ICategoryRepository categoryRepository,
            IHistoryService historyService,
            ILogService logService,
            IUserService userService,
            IPermissionAuthorizationService permissionAuthorizationService)
        {
            _categoryRepository = categoryRepository;
            _historyService = historyService;
            _logService = logService;
            _userService = userService;
            _permissionAuthorizationService = permissionAuthorizationService;
        }

        public async Task<Unit> Handle(SoftDeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userId = await _userService.GetCurrentUserIdAsync();
                if (!await _permissionAuthorizationService.HasPermissionAsync(userId, "CATEGORY_DELETE"))
                {
                    await _logService.CreateLog(
                        "Yetki Hatası",
                        $"Kullanıcı ID: {userId}, İzin: CATEGORY_DELETE, İşlem: Kategori Silme",
                        "Error",
                        "Authorization"
                    );
                    throw new AuFrameWorkException("Yetkiniz yok", "PERMISSION_DENIED", "Authorization");
                }

                var category = await _categoryRepository.GetByIdWithDetailsAsync(request.Id);
                if (category == null)
                {
                    throw new AuFrameWorkException(
                        "Kategori bulunamadı",
                        "CATEGORY_NOT_FOUND",
                        "NotFound"
                    );
                }

                if (category.News != null && category.News.Any())
                {
                    throw new AuFrameWorkException(
                        "Bu kategoriye bağlı haberler olduğu için silinemez",
                        "CATEGORY_HAS_NEWS",
                        "ValidationError"
                    );
                }

                category.IsDeleted = true;
                category.LastModifiedDate = DateTime.UtcNow;
                category.LastModifiedByUserId = userId;

                await _categoryRepository.UpdateAsync(category);
                await _historyService.SaveHistory(category, "SoftDelete");

                await _logService.CreateLog(
                    "Kategori Silindi",
                    $"Kullanıcı ID: {userId}, Kategori ID: {request.Id}",
                    "Information",
                    "Category"
                );

                return Unit.Value;
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "DeleteCategory",
                    "Kategori silinirken hata oluştu"
                );
                throw new AuFrameWorkException(
                    "Kategori silinirken bir hata oluştu",
                    "DELETE_ERROR",
                    "Error"
                );
            }
        }
    }
} 