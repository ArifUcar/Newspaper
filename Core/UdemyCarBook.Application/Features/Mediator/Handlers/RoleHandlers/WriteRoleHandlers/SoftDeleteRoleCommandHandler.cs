using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Commands.RoleCommands;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Application.Interfaces.IService;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Domain.Exceptions;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.RoleHandlers.WriteRoleHandlers
{
    public class SoftDeleteRoleCommandHandler : IRequestHandler<SoftDeleteRoleCommand>
    {
        private readonly IRoleRepository _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public SoftDeleteRoleCommandHandler(IRoleRepository repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task Handle(SoftDeleteRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _repository.GetByIdWithDetailsAsync(request.Id);
                if (role == null)
                    throw new AuFrameWorkException("Rol bulunamadı", "ROLE_NOT_FOUND", "NotFound");

                if (role.Users != null && role.Users.Any())
                    throw new AuFrameWorkException("Bu role atanmış kullanıcılar var. Önce kullanıcıları başka bir role atayın.", "ROLE_HAS_USERS", "ValidationError");

                role.IsDeleted = true;
                role.LastModifiedDate = DateTime.UtcNow;

                await _repository.UpdateAsync(role);
                await _historyService.SaveHistory(role, "SoftDelete");
                
                await _logService.CreateLog(
                    "Rol Yumuşak Silme",
                    $"'{role.Name}' adlı rol yumuşak silindi",
                    "SoftDelete",
                    "Role"
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "RoleSoftDelete",
                    $"Rol yumuşak silinirken hata: {request.Id}"
                );
                throw new AuFrameWorkException(
                    "Rol yumuşak silinirken bir hata oluştu", 
                    "SOFT_DELETE_ERROR",
                    "Error"
                );
            }
        }
    }
} 