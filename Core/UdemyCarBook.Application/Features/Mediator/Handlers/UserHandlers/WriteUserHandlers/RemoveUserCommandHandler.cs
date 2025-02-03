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
    public class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand>
    {
        private readonly IRepository<User> _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public RemoveUserCommandHandler(IRepository<User> repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task Handle(RemoveUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _repository.GetByIdAsync(request.Id);
                if (user == null)
                    throw new AuFrameWorkException("Kullanıcı bulunamadı", "USER_NOT_FOUND", "NotFound");

                await _repository.RemoveAsync(user);
                await _historyService.SaveHistory(user, "Remove");
                
                await _logService.CreateLog(
                    "Kullanıcı Silme",
                    $"'{user.UserName}' kullanıcı adlı kullanıcı silindi",
                    "Remove",
                    "User"
                );
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "UserRemove",
                    $"Kullanıcı silinirken hata: {request.Id}"
                );
                throw new AuFrameWorkException(
                    "Kullanıcı silinirken bir hata oluştu", 
                    "REMOVE_ERROR",
                    "Error"
                );
            }
        }
    }
} 