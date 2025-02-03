using MediatR;
using UdemyCarBook.Application.Features.Mediator.Results.UserResults;

namespace UdemyCarBook.Application.Features.Mediator.Commands.UserCommands
{
    public class RefreshTokenCommand : IRequest<LoginUserCommandResult>
    {
        public string RefreshToken { get; set; }
    }
} 