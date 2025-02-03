using MediatR;

namespace UdemyCarBook.Application.Features.Mediator.Commands.NewsletterCommands
{
    public class SoftDeleteNewsletterCommand : IRequest
    {
        public Guid Id { get; set; }
    }
} 