using MediatR;
using System.Collections.Generic;
using UdemyCarBook.Application.Features.Mediator.Results.CommentResults;

namespace UdemyCarBook.Application.Features.Mediator.Queries.CommentQueries
{
    public class GetCommentQuery : IRequest<List<GetCommentQueryResult>>
    {
    }
} 