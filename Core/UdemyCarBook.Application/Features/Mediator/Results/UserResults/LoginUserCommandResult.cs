using System;
using UdemyCarBook.Domain.Enums;

namespace UdemyCarBook.Application.Features.Mediator.Results.UserResults
{
    public class LoginUserCommandResult
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
        public string UserName { get; set; }
        public UserType UserType { get; set; }
        public Guid UserId { get; set; }
    }
} 