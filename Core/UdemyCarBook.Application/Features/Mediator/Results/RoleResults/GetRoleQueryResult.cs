using System;
using System.Collections.Generic;

namespace UdemyCarBook.Application.Features.Mediator.Results.RoleResults
{
    public class GetRoleQueryResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int UserCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByUserName { get; set; }
    }
} 