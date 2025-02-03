using MediatR;
using System;

namespace UdemyCarBook.Application.Features.Mediator.Commands.ContactCommands
{
    public class RemoveContactCommand : IRequest
    {
        public Guid Id { get; set; }
    }
} 