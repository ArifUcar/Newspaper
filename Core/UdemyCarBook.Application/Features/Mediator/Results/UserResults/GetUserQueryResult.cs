using System;
using System.Collections.Generic;
using UdemyCarBook.Domain.Enums;

namespace UdemyCarBook.Application.Features.Mediator.Results.UserResults
{
    public class GetUserQueryResult
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public UserType UserType { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByUserName { get; set; }
        public List<string> Roles { get; set; }
        public int NewsCount { get; set; }
        public int CommentsCount { get; set; }
    }
}