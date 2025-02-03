using MediatR;
using System;

namespace UdemyCarBook.Application.Features.Mediator.Commands.UserCommands
{
    public class SoftDeleteUserCommand : IRequest
    {
        public SoftDeleteUserCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
} 