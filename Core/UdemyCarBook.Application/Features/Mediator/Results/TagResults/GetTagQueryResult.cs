using System;
using System.Collections.Generic;

namespace UdemyCarBook.Application.Features.Mediator.Results.TagResults
{
    public class GetTagQueryResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public int NewsCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByUserName { get; set; }
    }
} 