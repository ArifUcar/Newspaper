using MediatR;
using System;

namespace UdemyCarBook.Application.Features.Mediator.Commands.PermissionCommands
{
    public class DeletePermissionCommand : IRequest
    {
        public Guid Id { get; set; }
    }
} 