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
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Unit>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;
        private readonly IUserService _userService;
        private readonly IPermissionAuthorizationService _permissionAuthorizationService;

        public UpdateCategoryCommandHandler(
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

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userId = await _userService.GetCurrentUserIdAsync();
                if (!await _permissionAuthorizationService.HasPermissionAsync(userId, "CATEGORY_UPDATE"))
                {
                    await _logService.CreateLog(
                        "Yetki Hatası",
                        $"Kullanıcı ID: {userId}, İzin: CATEGORY_UPDATE, İşlem: Kategori Güncelleme",
                        "Error",
                        "Authorization"
                    );
                    throw new AuFrameWorkException("Yetkiniz yok", "PERMISSION_DENIED", "Authorization");
                }

                var category = await _categoryRepository.GetByIdAsync(request.Id);
                if (category == null)
                {
                    throw new AuFrameWorkException(
                        "Kategori bulunamadı",
                        "CATEGORY_NOT_FOUND",
                        "NotFound"
                    );
                }

                if (category.Name != request.Name && await _categoryRepository.IsCategoryExistsAsync(request.Name))
                {
                    throw new AuFrameWorkException(
                        "Bu isimde bir kategori zaten mevcut",
                        "CATEGORY_EXISTS",
                        "ValidationError"
                    );
                }

                category.Name = request.Name;
                category.Description = request.Description;
                category.IconUrl = request.IconUrl;
                category.LastModifiedDate = DateTime.UtcNow;
                category.LastModifiedByUserId = userId;

                await _categoryRepository.UpdateAsync(category);
                await _historyService.SaveHistory(category, "Update");

                await _logService.CreateLog(
                    "Kategori Güncellendi",
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
                    "UpdateCategory",
                    "Kategori güncellenirken hata oluştu"
                );
                throw new AuFrameWorkException(
                    "Kategori güncellenirken bir hata oluştu",
                    "UPDATE_ERROR",
                    "Error"
                );
            }
        }
    }
} 