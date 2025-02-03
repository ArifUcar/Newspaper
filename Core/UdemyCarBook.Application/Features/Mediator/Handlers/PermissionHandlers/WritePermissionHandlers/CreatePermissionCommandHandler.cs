using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Commands.PermissionCommands;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Application.Interfaces.IService;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Domain.Exceptions;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.PermissionHandlers.WritePermissionHandlers
{
    public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand>
    {
        private readonly IPermissionRepository _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;
        private readonly IUserService _userService;
        private readonly IPermissionLogService _permissionLogService;

        public CreatePermissionCommandHandler(
            IPermissionRepository repository,
            IHistoryService historyService,
            ILogService logService,
            IUserService userService,
            IPermissionLogService permissionLogService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
            _userService = userService;
            _permissionLogService = permissionLogService;
        }

        public async Task Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Name))
                    throw new AuFrameWorkException("Yetki adı boş olamaz", "NAME_REQUIRED", "ValidationError");

                if (string.IsNullOrEmpty(request.Code))
                    throw new AuFrameWorkException("Yetki kodu boş olamaz", "CODE_REQUIRED", "ValidationError");

                var isPermissionCodeExists = await _repository.IsPermissionCodeExistsAsync(request.Code);
                if (isPermissionCodeExists)
                    throw new AuFrameWorkException("Bu yetki kodu zaten kullanılıyor", "CODE_EXISTS", "ValidationError");

                var currentUser = await _userService.GetCurrentUserAsync();
                if (currentUser == null)
                    throw new AuFrameWorkException("Oturum açmış kullanıcı bulunamadı", "USER_NOT_FOUND", "NotFound");

                var permission = new Permission
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description,
                    Group = request.Group,
                    Code = request.Code.ToUpper(),
                    CreatedById = currentUser.Id,
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false,
                    IsActive = true,
                    UpdatedByUserId = null,
                    LastModifiedByUserId = null
                };

                await _repository.CreateAsync(permission);
                await _historyService.SaveHistory(permission, "Create");
                
                await _logService.CreateLog(
                    "Yetki Oluşturma",
                    $"'{request.Name}' adlı yetki oluşturuldu",
                    "Create",
                    "Permission"
                );

                await _permissionLogService.LogPermissionCreated(
                    permission.Name,
                    permission.Code,
                    currentUser.Id
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "PermissionCreate",
                    $"Yetki oluşturulurken hata: {request.Name}"
                );
                throw new AuFrameWorkException(
                    "Yetki oluşturulurken bir hata oluştu", 
                    "CREATE_ERROR",
                    "Error"
                );
            }
        }
    }
} 