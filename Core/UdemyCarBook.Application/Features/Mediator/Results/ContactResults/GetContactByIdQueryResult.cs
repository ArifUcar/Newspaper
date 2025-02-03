using System;

namespace UdemyCarBook.Application.Features.Mediator.Results.ContactResults
{
    public class GetContactByIdQueryResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public bool IsReplied { get; set; }
        public DateTime? ReplyDate { get; set; }
        public string? ReplyMessage { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByUserName { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string? LastModifiedByUserName { get; set; }
    }
} 