using System;
using System.Collections.Generic;

namespace UdemyCarBook.Application.Features.Mediator.Results.CommentResults
{
    public class GetCommentQueryResult
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsApproved { get; set; }
        public Guid NewsId { get; set; }
        public string NewsTitle { get; set; }
        public Guid? ParentCommentId { get; set; }
        public string ParentCommentContent { get; set; }
        public int ReplyCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByUserName { get; set; }
    }
} 