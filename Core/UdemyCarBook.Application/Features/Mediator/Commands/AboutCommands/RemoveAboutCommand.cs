using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemyCarBook.Application.Features.Mediator.Commands.AboutCommands
{
    public class RemoveAboutCommand :IRequest
    {
        public RemoveAboutCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }

    }
}
