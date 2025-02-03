using System;
using System.Collections.Generic;
using UdemyCarBook.Domain.Entities;

namespace UdemyCarBook.Application.Features.Mediator.Results.NewsResults
{
    public class GetNewsByIdQueryResult
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string ImageUrl { get; set; }
        public string Summary { get; set; }
        public int ViewCount { get; set; }
        public NewsStatus Status { get; set; }
        public List<string> Tags { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByUserName { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string LastModifiedByUserName { get; set; }
    }
} 