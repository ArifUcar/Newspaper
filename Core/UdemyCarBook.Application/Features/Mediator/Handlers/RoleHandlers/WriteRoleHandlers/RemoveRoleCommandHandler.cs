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
    public class RemoveRoleCommandHandler : IRequestHandler<RemoveRoleCommand>
    {
        private readonly IRoleRepository _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public RemoveRoleCommandHandler(IRoleRepository repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _repository.GetByIdWithDetailsAsync(request.Id);
                if (role == null)
                    throw new AuFrameWorkException("Rol bulunamadı", "ROLE_NOT_FOUND", "NotFound");

                if (role.Users != null && role.Users.Any())
                    throw new AuFrameWorkException("Bu role atanmış kullanıcılar var. Önce kullanıcıları başka bir role atayın.", "ROLE_HAS_USERS", "ValidationError");

                await _repository.RemoveAsync(role);
                await _historyService.SaveHistory(role, "Remove");
                
                await _logService.CreateLog(
                    "Rol Silme",
                    $"'{role.Name}' adlı rol silindi",
                    "Remove",
                    "Role"
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "RoleRemove",
                    $"Rol silinirken hata: {request.Id}"
                );
                throw new AuFrameWorkException(
                    "Rol silinirken bir hata oluştu", 
                    "REMOVE_ERROR",
                    "Error"
                );
            }
        }
    }
} 