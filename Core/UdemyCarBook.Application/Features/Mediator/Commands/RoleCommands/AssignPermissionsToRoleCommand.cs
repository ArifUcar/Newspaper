using MediatR;
using System;
using System.Collections.Generic;

namespace UdemyCarBook.Application.Features.Mediator.Commands.RoleCommands
{
    public class AssignPermissionsToRoleCommand : IRequest
    {
        public Guid RoleId { get; set; }
        public List<Guid> PermissionIds { get; set; }
    }
} 