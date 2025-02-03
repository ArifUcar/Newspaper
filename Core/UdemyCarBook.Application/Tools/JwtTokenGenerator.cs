using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UdemyCarBook.Domain.Entities;

namespace UdemyCarBook.Application.Tools
{
    public class JwtTokenGenerator
    {
        public static TokenResponseDto GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName ?? string.Empty),
                new Claim(ClaimTypes.Surname, user.LastName ?? string.Empty),
                new Claim("UserType", user.UserType.ToString())
            };

            if (user.Roles != null)
            {
                foreach (var role in user.Roles)
                {
                    if (role.IsActive && !role.IsDeleted)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.Name));
                    }
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtTokenDefaults.Key));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expireDate = DateTime.UtcNow.AddMinutes(JwtTokenDefaults.Expire);

            var token = new JwtSecurityToken(
                issuer: JwtTokenDefaults.ValidIssuer,
                audience: JwtTokenDefaults.ValidAudience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expireDate,
                signingCredentials: signingCredentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();

            // Aktif rolleri al
            var roles = user.Roles?
                .Where(r => r.IsActive && !r.IsDeleted)
                .Select(r => r.Name)
                .ToList() ?? new List<string>();

            return new TokenResponseDto
            {
                Token = tokenHandler.WriteToken(token),
                Expiration = expireDate,
                RefreshToken = Guid.NewGuid().ToString(),
                Roles = roles
            };
        }
    }
} 