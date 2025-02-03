using MediatR;
using UdemyCarBook.Application.Tools;

namespace UdemyCarBook.Application.Features.Mediator.Commands.AuthCommands
{
    public class LoginCommand : IRequest<TokenResponseDto>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
} 