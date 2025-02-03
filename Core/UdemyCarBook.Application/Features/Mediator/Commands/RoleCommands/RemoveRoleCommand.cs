using MediatR;
using System;

namespace UdemyCarBook.Application.Features.Mediator.Commands.RoleCommands
{
    public class RemoveRoleCommand : IRequest
    {
        public RemoveRoleCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
} 