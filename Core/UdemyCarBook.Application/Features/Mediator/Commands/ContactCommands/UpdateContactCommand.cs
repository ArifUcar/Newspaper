using MediatR;
using System;

namespace UdemyCarBook.Application.Features.Mediator.Commands.ContactCommands
{
    public class UpdateContactCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public bool IsReplied { get; set; }
        public DateTime? ReplyDate { get; set; }
        public string? ReplyMessage { get; set; }
    }
} 