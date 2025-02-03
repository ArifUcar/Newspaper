using MediatR;
using System;

namespace UdemyCarBook.Application.Features.Mediator.Commands.ContactCommands
{
    public class MarkContactAsReadCommand : IRequest
    {
        public Guid Id { get; set; }
    }
} 