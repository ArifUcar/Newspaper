using MediatR;
using System;

namespace UdemyCarBook.Application.Features.Mediator.Commands.TagCommands
{
    public class RemoveTagCommand : IRequest
    {
        public Guid Id { get; set; }
    }
} 