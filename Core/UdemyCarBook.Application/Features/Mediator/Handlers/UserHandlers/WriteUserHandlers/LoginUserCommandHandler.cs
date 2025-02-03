using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using UdemyCarBook.Application.Features.Mediator.Commands.UserCommands;
using UdemyCarBook.Application.Features.Mediator.Results.UserResults;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Application.Interfaces.IService;
using UdemyCarBook.Application.Tools;
using UdemyCarBook.Domain.Exceptions;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.UserHandlers.WriteUserHandlers
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserCommandResult>
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHashService _passwordHashService;
        private readonly ILogService _logService;

        public LoginUserCommandHandler(
            IUserRepository repository,
            IPasswordHashService passwordHashService,
            ILogService logService)
        {
            _repository = repository;
            _passwordHashService = passwordHashService;
            _logService = logService;
        }

        public async Task<LoginUserCommandResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _repository.GetByUsernameAsync(request.UserName);
                if (user == null)
                    throw new AuFrameWorkException("Kullanıcı adı veya şifre hatalı", "LOGIN_FAILED", "ValidationError");

                if (!user.IsActive)
                    throw new AuFrameWorkException("Hesabınız aktif değil", "ACCOUNT_INACTIVE", "ValidationError");

                var isPasswordValid = await _passwordHashService.VerifyPasswordAsync(
                    request.Password,
                    user.PasswordHash,
                    user.PasswordSalt
                );

                if (!isPasswordValid)
                    throw new AuFrameWorkException("Kullanıcı adı veya şifre hatalı", "LOGIN_FAILED", "ValidationError");

                // Token oluştur
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(JwtTokenDefaults.Key);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim("UserType", user.UserType.ToString())
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
                if (user.Roles != null)
                {
                    foreach (var role in user.Roles)
                    {
                        ((ClaimsIdentity)tokenDescriptor.Subject).AddClaim(new Claim(ClaimTypes.Role, role.Name));
                    }
                }

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                // Refresh token oluştur
                var refreshToken = Guid.NewGuid().ToString();
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpireDate = DateTime.UtcNow.AddDays(7);
                user.LastLoginDate = DateTime.UtcNow;

                await _repository.UpdateAsync(user);

                await _logService.CreateLog(
                    "Kullanıcı Girişi",
                    $"'{user.UserName}' kullanıcısı giriş yaptı",
                    "Login",
                    "User"
                );

                return new LoginUserCommandResult
                {
                    Token = tokenString,
                    RefreshToken = refreshToken,
                    Expiration = tokenDescriptor.Expires.Value,
                    UserName = user.UserName,
                    UserType = user.UserType,
                    UserId = user.Id
                };
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "UserLogin",
                    $"Kullanıcı girişi yapılırken hata: {ex.Message}"
                );
                throw new AuFrameWorkException(
                    "Giriş yapılırken bir hata oluştu",
                    "LOGIN_ERROR",
                    "Error"
                );
            }
        }
    }
} 