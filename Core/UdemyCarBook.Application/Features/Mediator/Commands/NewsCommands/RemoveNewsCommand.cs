using MediatR;
using System;

namespace UdemyCarBook.Application.Features.Mediator.Commands.NewsCommands
{
    public class RemoveNewsCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}