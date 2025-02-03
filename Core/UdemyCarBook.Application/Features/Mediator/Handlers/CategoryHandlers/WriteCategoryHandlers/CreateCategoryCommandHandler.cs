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
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Unit>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;
        private readonly IUserService _userService;
        private readonly IPermissionAuthorizationService _permissionAuthorizationService;

        public CreateCategoryCommandHandler(
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

        public async Task<Unit> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userId = await _userService.GetCurrentUserIdAsync();
                if (!await _permissionAuthorizationService.HasPermissionAsync(userId, "CATEGORY_CREATE"))
                {
                    await _logService.CreateLog(
                        "Yetki Hatası",
                        $"Kullanıcı ID: {userId}, İzin: CATEGORY_CREATE, İşlem: Kategori Oluşturma",
                        "Error",
                        "Authorization"
                    );
                    throw new AuFrameWorkException("Yetkiniz yok", "PERMISSION_DENIED", "Authorization");
                }

                if (await _categoryRepository.IsCategoryExistsAsync(request.Name))
                {
                    throw new AuFrameWorkException(
                        "Bu isimde bir kategori zaten mevcut",
                        "CATEGORY_EXISTS",
                        "ValidationError"
                    );
                }

                var category = new Category
                {
                    Name = request.Name,
                    Description = request.Description,
                    IconUrl = request.IconUrl,
                    CreatedDate = DateTime.UtcNow,
                    CreatedById = userId,
                    LastModifiedDate = DateTime.UtcNow,
                    LastModifiedByUserId = userId
                };

                await _categoryRepository.CreateAsync(category);
                await _historyService.SaveHistory(category, "Create");

                await _logService.CreateLog(
                    "Kategori Oluşturuldu",
                    $"Kullanıcı ID: {userId}, Kategori: {request.Name}",
                    "Information",
                    "Category"
                );

                return Unit.Value;
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "CreateCategory",
                    "Kategori oluşturulurken hata oluştu"
                );
                throw new AuFrameWorkException(
                    "Kategori oluşturulurken bir hata oluştu",
                    "CREATE_ERROR",
                    "Error"
                );
            }
        }
    }
} 