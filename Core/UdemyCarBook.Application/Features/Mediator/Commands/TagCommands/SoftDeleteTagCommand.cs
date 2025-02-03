using MediatR;
using System;

namespace UdemyCarBook.Application.Features.Mediator.Commands.TagCommands
{
    public class SoftDeleteTagCommand : IRequest
    {
        public Guid Id { get; set; }
    }
} 