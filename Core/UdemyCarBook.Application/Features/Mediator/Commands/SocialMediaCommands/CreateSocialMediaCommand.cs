using MediatR;
using System;

namespace UdemyCarBook.Application.Features.Mediator.Commands.SocialMediaCommands
{
    public class CreateSocialMediaCommand : IRequest
    {
        public string Platform { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public int? FollowerCount { get; set; }
        public string? AccountName { get; set; }
        public Guid? UserId { get; set; }
    }
} 