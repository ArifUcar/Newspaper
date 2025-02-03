using System;
using System.Collections.Generic;

namespace UdemyCarBook.Application.Tools
{
    public class TokenResponseDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string RefreshToken { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
} 