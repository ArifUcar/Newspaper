using MediatR;
using UdemyCarBook.Application.Features.Mediator.Results.UserResults;

namespace UdemyCarBook.Application.Features.Mediator.Commands.UserCommands
{
    public class LoginUserCommand : IRequest<LoginUserCommandResult>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
} 