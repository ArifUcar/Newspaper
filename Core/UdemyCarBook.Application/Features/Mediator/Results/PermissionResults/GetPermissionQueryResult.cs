using System;

namespace UdemyCarBook.Application.Features.Mediator.Results.PermissionResults
{
    public class GetPermissionQueryResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Group { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public int RoleCount { get; set; }
    }
} 