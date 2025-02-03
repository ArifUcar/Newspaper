using MediatR;
using System;

namespace UdemyCarBook.Application.Features.Mediator.Commands.ContactCommands
{
    public class ReplyContactCommand : IRequest
    {
        public Guid Id { get; set; }
        public string ReplyMessage { get; set; }
    }
} 