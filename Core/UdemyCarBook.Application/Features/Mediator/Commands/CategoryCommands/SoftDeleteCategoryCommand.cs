using MediatR;
using System;

namespace UdemyCarBook.Application.Features.Mediator.Commands.CategoryCommands
{
    public class SoftDeleteCategoryCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
} 