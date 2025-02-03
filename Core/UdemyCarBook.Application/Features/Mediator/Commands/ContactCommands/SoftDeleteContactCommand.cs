using MediatR;
using System;

namespace UdemyCarBook.Application.Features.Mediator.Commands.ContactCommands
{
    public class SoftDeleteContactCommand : IRequest
    {
        public Guid Id { get; set; }
    }
} 