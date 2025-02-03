using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UdemyCarBook.Application.Features.Mediator.Commands.UserCommands;
using UdemyCarBook.Application.Features.Mediator.Results.UserResults;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Application.Interfaces.IService;
using UdemyCarBook.Application.Tools;
using UdemyCarBook.Domain.Exceptions;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.UserHandlers.WriteUserHandlers
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginUserCommandResult>
    {
        private readonly IUserRepository _repository;
        private readonly ILogService _logService;

        public RefreshTokenCommandHandler(IUserRepository repository, ILogService logService)
        {
            _repository = repository;
            _logService = logService;
        }

        public async Task<LoginUserCommandResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _repository.GetAllWithDetailsAsync();
                var currentUser = user.FirstOrDefault(x => x.RefreshToken == request.RefreshToken);

                if (currentUser == null)
                    throw new AuFrameWorkException("Geçersiz refresh token", "INVALID_REFRESH_TOKEN", "ValidationError");

                if (currentUser.RefreshTokenExpireDate <= DateTime.UtcNow)
                    throw new AuFrameWorkException("Refresh token süresi dolmuş", "REFRESH_TOKEN_EXPIRED", "ValidationError");

                // Yeni token oluştur
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(JwtTokenDefaults.Key);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, currentUser.Id.ToString()),
                        new Claim(ClaimTypes.Name, currentUser.UserName),
                        new Claim(ClaimTypes.Email, currentUser.Email),
                        new Claim("UserType", currentUser.UserType.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(JwtTokenDefaults.Expire),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature
                    ),
                    Issuer = JwtTokenDefaults.ValidIssuer,
                    Audience = JwtTokenDefaults.ValidAudience
                };

                // Rolleri ekle
                if (currentUser.Roles != null)
                {
                    foreach (var role in currentUser.Roles)
                    {
                        ((ClaimsIdentity)tokenDescriptor.Subject).AddClaim(new Claim(ClaimTypes.Role, role.Name));
                    }
                }

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                // Yeni refresh token oluştur
                var refreshToken = Guid.NewGuid().ToString();
                currentUser.RefreshToken = refreshToken;
                currentUser.RefreshTokenExpireDate = DateTime.UtcNow.AddDays(7);

                await _repository.UpdateAsync(currentUser);

                await _logService.CreateLog(
                    "Token Yenileme",
                    $"'{currentUser.UserName}' kullanıcısının token'ı yenilendi",
                    "RefreshToken",
                    "User"
                );

                return new LoginUserCommandResult
                {
                    Token = tokenString,
                    RefreshToken = refreshToken,
                    Expiration = tokenDescriptor.Expires.Value,
                    UserName = currentUser.UserName,
                    UserType = currentUser.UserType,
                    UserId = currentUser.Id
                };
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "RefreshToken",
                    $"Token yenilenirken hata: {ex.Message}"
                );
                throw new AuFrameWorkException(
                    "Token yenilenirken bir hata oluştu",
                    "REFRESH_TOKEN_ERROR",
                    "Error"
                );
            }
        }
    }
} 