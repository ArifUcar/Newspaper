using MediatR;

namespace UdemyCarBook.Application.Features.Mediator.Commands.NewsletterCommands
{
    public class UpdateNewsletterCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UnsubscribeDate { get; set; }
    }
} 