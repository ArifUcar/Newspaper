using MediatR;

namespace UdemyCarBook.Application.Features.Mediator.Commands.ContactCommands
{
    public class CreateContactCommand : IRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool SendCopy { get; set; }
    }
} 