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
    public class DeletePermissionCommandHandler : IRequestHandler<DeletePermissionCommand>
    {
        private readonly IPermissionRepository _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;
        private readonly IUserService _userService;

        public DeletePermissionCommandHandler(
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

        public async Task Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var permission = await _repository.GetByIdWithDetailsAsync(request.Id);
                if (permission == null)
                    throw new AuFrameWorkException("Yetki bulunamadı", "PERMISSION_NOT_FOUND", "NotFound");

                if (permission.Roles != null && permission.Roles.Count > 0)
                    throw new AuFrameWorkException("Bu yetkiye atanmış roller var. Önce rolleri başka bir yetkiye atayın.", "PERMISSION_HAS_ROLES", "ValidationError");

                var currentUser = await _userService.GetCurrentUserAsync();
                if (currentUser == null)
                    throw new AuFrameWorkException("Oturum açmış kullanıcı bulunamadı", "USER_NOT_FOUND", "NotFound");

                permission.LastModifiedDate = DateTime.UtcNow;
                permission.LastModifiedByUserId = currentUser.Id;
                permission.UpdatedByUserId = currentUser.Id;
                

                await _repository.RemoveAsync(permission);
                await _historyService.SaveHistory(permission, "Delete");
                
                await _logService.CreateLog(
                    "Yetki Silme",
                    $"'{permission.Name}' adlı yetki silindi",
                    "Delete",
                    "Permission"
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "PermissionDelete",
                    $"Yetki silinirken hata: {request.Id}"
                );
                throw new AuFrameWorkException(
                    "Yetki silinirken bir hata oluştu", 
                    "DELETE_ERROR",
                    "Error"
                );
            }
        }
    }
} 