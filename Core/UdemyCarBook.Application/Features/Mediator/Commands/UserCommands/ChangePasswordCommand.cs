using MediatR;
using System;

namespace UdemyCarBook.Application.Features.Mediator.Commands.UserCommands
{
    public class ChangePasswordCommand : IRequest
    {
        public Guid Id { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
} 