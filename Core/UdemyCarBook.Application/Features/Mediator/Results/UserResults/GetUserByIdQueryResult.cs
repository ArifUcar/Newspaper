using System;
using System.Collections.Generic;

namespace UdemyCarBook.Application.Features.Mediator.Results.UserResults
{
    public class GetUserByIdQueryResult
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public List<string> Roles { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByUserName { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string LastModifiedByUserName { get; set; }
        public List<NewsInfo> CreatedNews { get; set; }
        public List<NewsInfo> UpdatedNews { get; set; }
        public List<CommentInfo> CreatedComments { get; set; }
        public List<CommentInfo> UpdatedComments { get; set; }
    }

    public class NewsInfo
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CommentInfo
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string NewsTitle { get; set; }
        public bool IsApproved { get; set; }
    }
} 