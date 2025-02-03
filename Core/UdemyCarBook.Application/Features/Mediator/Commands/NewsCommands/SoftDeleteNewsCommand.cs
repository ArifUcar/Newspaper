using MediatR;
using System;

namespace UdemyCarBook.Application.Features.Mediator.Commands.NewsCommands
{
    public class SoftDeleteNewsCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public Guid LastModifiedById { get; set; }
    }
}