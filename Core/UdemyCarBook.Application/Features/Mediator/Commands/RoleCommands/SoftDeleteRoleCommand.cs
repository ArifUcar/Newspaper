using MediatR;
using System;

namespace UdemyCarBook.Application.Features.Mediator.Commands.RoleCommands
{
    public class SoftDeleteRoleCommand : IRequest
    {
        public Guid Id { get; set; }
    }
} 