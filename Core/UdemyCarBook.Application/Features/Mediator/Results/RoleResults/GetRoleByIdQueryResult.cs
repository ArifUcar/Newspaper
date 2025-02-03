using System;
using System.Collections.Generic;

namespace UdemyCarBook.Application.Features.Mediator.Results.RoleResults
{
    public class GetRoleByIdQueryResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<UserInRoleDto> Users { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByUserName { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string LastModifiedByUserName { get; set; }
    }

    public class UserInRoleDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
} 