using MediatR;

namespace UdemyCarBook.Application.Features.Mediator.Commands.NewsletterCommands
{
    public class RemoveNewsletterCommand : IRequest
    {
        public Guid Id { get; set; }
    }
} 