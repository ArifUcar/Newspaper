using System;
using System.Collections.Generic;

namespace UdemyCarBook.Application.Features.Mediator.Results.TagResults
{
    public class GetTagByIdQueryResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public List<TagNewsDto> News { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByUserName { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string LastModifiedByUserName { get; set; }
    }

    public class TagNewsDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public string CategoryName { get; set; }
        public string AuthorName { get; set; }
    }
} 