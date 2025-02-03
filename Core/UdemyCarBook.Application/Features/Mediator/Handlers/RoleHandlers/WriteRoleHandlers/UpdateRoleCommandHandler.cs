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
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand>
    {
        private readonly IRoleRepository _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;
        private readonly IUserService _userService;

        public UpdateRoleCommandHandler(
            IRoleRepository repository, 
            IHistoryService historyService, 
            ILogService logService,
            IUserService userService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
            _userService = userService;
        }

        public async Task Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _repository.GetByIdWithDetailsAsync(request.Id);
                if (role == null)
                    throw new AuFrameWorkException("Rol bulunamadı", "ROLE_NOT_FOUND", "NotFound");

                if (string.IsNullOrEmpty(request.Name))
                    throw new AuFrameWorkException("Rol adı boş olamaz", "NAME_REQUIRED", "ValidationError");

                if (role.Name != request.Name)
                {
                    var isRoleNameExists = await _repository.IsRoleNameExistsAsync(request.Name);
                    if (isRoleNameExists)
                        throw new AuFrameWorkException("Bu rol adı zaten kullanılıyor", "ROLE_NAME_EXISTS", "ValidationError");
                }

                var currentUser = await _userService.GetCurrentUserAsync();
                if (currentUser == null)
                    throw new AuFrameWorkException("Oturum açmış kullanıcı bulunamadı", "USER_NOT_FOUND", "NotFound");

                role.Name = request.Name;
                role.Description = request.Description;
                role.LastModifiedDate = DateTime.UtcNow;
                role.LastModifiedByUserId = currentUser.Id;
                role.UpdatedByUserId = currentUser.Id;

                await _repository.UpdateAsync(role);
                await _historyService.SaveHistory(role, "Update");
                
                await _logService.CreateLog(
                    "Rol Güncelleme",
                    $"'{request.Name}' adlı rol güncellendi",
                    "Update",
                    "Role"
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "RoleUpdate",
                    $"Rol güncellenirken hata: {request.Name}"
                );
                throw new AuFrameWorkException(
                    "Rol güncellenirken bir hata oluştu", 
                    "UPDATE_ERROR",
                    "Error"
                );
            }
        }
    }
} 