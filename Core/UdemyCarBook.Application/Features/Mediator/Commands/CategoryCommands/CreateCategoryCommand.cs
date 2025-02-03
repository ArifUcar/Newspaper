using MediatR;

namespace UdemyCarBook.Application.Features.Mediator.Commands.CategoryCommands
{
    public class CreateCategoryCommand : IRequest<Unit>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? IconUrl { get; set; }
    }
} 