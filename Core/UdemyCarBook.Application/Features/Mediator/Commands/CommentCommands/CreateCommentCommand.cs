using MediatR;
using System;

namespace UdemyCarBook.Application.Features.Mediator.Commands.CommentCommands
{
    public class CreateCommentCommand : IRequest
    {
        public string Content { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsApproved { get; set; }
        public Guid NewsId { get; set; }
        public Guid? ParentCommentId { get; set; }
    }
} 