using MediatR;

namespace UdemyCarBook.Application.Features.Mediator.Commands.PermissionCommands
{
    public class CreatePermissionCommand : IRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Group { get; set; }
        public string Code { get; set; }
    }
} 