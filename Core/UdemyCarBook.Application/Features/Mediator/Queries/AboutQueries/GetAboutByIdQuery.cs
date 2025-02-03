using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Results.AboutResults;

namespace UdemyCarBook.Application.Features.Mediator.Queries.AboutQueries
{
    public class GetAboutByIdQuery :IRequest<GetAboutByIdQueryResult>
    {
        public GetAboutByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
