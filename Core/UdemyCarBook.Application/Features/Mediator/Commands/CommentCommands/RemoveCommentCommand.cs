using MediatR;
using System;

namespace UdemyCarBook.Application.Features.Mediator.Commands.CommentCommands
{
    public class RemoveCommentCommand : IRequest
    {
        public Guid Id { get; set; }
    }
} 