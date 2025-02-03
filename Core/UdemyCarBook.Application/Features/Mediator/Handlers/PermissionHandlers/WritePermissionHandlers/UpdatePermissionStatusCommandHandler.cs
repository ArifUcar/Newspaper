using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Commands.PermissionCommands;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Application.Interfaces.IService;
using UdemyCarBook.Domain.Exceptions;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.PermissionHandlers.WritePermissionHandlers
{
    public class UpdatePermissionStatusCommandHandler : IRequestHandler<UpdatePermissionStatusCommand>
    {
        private readonly IPermissionRepository _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;
        private readonly IUserService _userService;

        public UpdatePermissionStatusCommandHandler(
            IPermissionRepository repository,
            IHistoryService historyService,
            ILogService logService,
            IUserService userService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
            _userService = userService;
        }

        public async Task Handle(UpdatePermissionStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var permission = await _repository.GetByIdWithDetailsAsync(request.Id);
                if (permission == null)
                    throw new AuFrameWorkException("Yetki bulunamadı", "PERMISSION_NOT_FOUND", "NotFound");

                var currentUser = await _userService.GetCurrentUserAsync();
                if (currentUser == null)
                    throw new AuFrameWorkException("Oturum açmış kullanıcı bulunamadı", "USER_NOT_FOUND", "NotFound");

                permission.IsActive = request.IsActive;
                permission.LastModifiedDate = DateTime.UtcNow;
                permission.LastModifiedByUserId = currentUser.Id;
                permission.UpdatedByUserId = currentUser.Id;

                await _repository.UpdateAsync(permission);
                await _historyService.SaveHistory(permission, "Update");
                
                var action = request.IsActive ? "aktif" : "pasif";
                await _logService.CreateLog(
                    "Yetki Durumu Güncelleme",
                    $"'{permission.Name}' adlı yetki {action} yapıldı",
                    "Update",
                    "Permission"
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "PermissionStatusUpdate",
                    $"Yetki durumu güncellenirken hata: {request.Id}"
                );
                throw new AuFrameWorkException(
                    "Yetki durumu güncellenirken bir hata oluştu", 
                    "UPDATE_ERROR",
                    "Error"
                );
            }
        }
    }
} 