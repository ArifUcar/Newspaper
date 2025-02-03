using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Commands.UserCommands;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Application.Interfaces.IService;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Domain.Exceptions;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.UserHandlers.WriteUserHandlers
{
    public class SoftDeleteUserCommandHandler : IRequestHandler<SoftDeleteUserCommand>
    {
        private readonly IRepository<User> _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public SoftDeleteUserCommandHandler(IRepository<User> repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task Handle(SoftDeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _repository.GetByIdAsync(request.Id);
                if (user == null)
                    throw new AuFrameWorkException("Kullanıcı bulunamadı", "USER_NOT_FOUND", "NotFound");

                user.IsDeleted = true;
                user.LastModifiedDate = DateTime.UtcNow;

                await _repository.UpdateAsync(user);
                await _historyService.SaveHistory(user, "SoftDelete");
                
                await _logService.CreateLog(
                    "Kullanıcı Yumuşak Silme",
                    $"'{user.UserName}' kullanıcı adlı kullanıcı yumuşak silindi",
                    "SoftDelete",
                    "User"
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "UserSoftDelete",
                    $"Kullanıcı yumuşak silinirken hata: {request.Id}"
                );
                throw new AuFrameWorkException(
                    "Kullanıcı yumuşak silinirken bir hata oluştu", 
                    "SOFT_DELETE_ERROR",
                    "Error"
                );
            }
        }
    }
} 