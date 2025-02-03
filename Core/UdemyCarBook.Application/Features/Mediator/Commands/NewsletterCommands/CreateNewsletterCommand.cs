using MediatR;

namespace UdemyCarBook.Application.Features.Mediator.Commands.NewsletterCommands
{
    public class CreateNewsletterCommand : IRequest
    {
        public string Email { get; set; }
        public bool IsActive { get; set; } = true;
    }
} 