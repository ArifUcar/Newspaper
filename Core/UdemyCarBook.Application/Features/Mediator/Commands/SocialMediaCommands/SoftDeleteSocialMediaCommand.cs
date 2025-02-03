using MediatR;
using System;

namespace UdemyCarBook.Application.Features.Mediator.Commands.SocialMediaCommands
{
    public class SoftDeleteSocialMediaCommand : IRequest
    {
        public Guid Id { get; set; }
    }
} 