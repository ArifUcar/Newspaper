using MediatR;
using System;
using System.Collections.Generic;

namespace UdemyCarBook.Application.Features.Mediator.Commands.NewsCommands
{
    public class CreateNewsCommand : IRequest<Unit>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Summary { get; set; }
        public string Slug { get; set; }
        public string CoverImageBase64 { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublished { get; set; }
        public DateTime PublishDate { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public Guid CategoryId { get; set; }
        public Guid UserId { get; set; }
        public List<Guid> TagIds { get; set; }
    }
}