using MediatR;
using System;

namespace UdemyCarBook.Application.Features.Mediator.Commands.PermissionCommands
{
    public class UpdatePermissionStatusCommand : IRequest
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
    }
} 