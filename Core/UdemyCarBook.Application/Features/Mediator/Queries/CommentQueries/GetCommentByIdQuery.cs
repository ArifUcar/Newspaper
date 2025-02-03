using MediatR;
using System;
using UdemyCarBook.Application.Features.Mediator.Results.CommentResults;

namespace UdemyCarBook.Application.Features.Mediator.Queries.CommentQueries
{
    public class GetCommentByIdQuery : IRequest<GetCommentByIdQueryResult>
    {
        public Guid Id { get; set; }
    }
} 