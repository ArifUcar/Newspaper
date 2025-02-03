using MediatR;

namespace UdemyCarBook.Application.Features.Mediator.Commands.TagCommands
{
    public class CreateTagCommand : IRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
    }
} 