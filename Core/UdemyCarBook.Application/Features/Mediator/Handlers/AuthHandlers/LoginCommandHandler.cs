using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Commands.AuthCommands;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Application.Tools;
using UdemyCarBook.Domain.Exceptions;
using UdemyCarBook.Application.Interfaces.IService;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.AuthHandlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHashService _passwordHashService;

        public LoginCommandHandler(IUserRepository userRepository, IPasswordHashService passwordHashService)
        {
            _userRepository = userRepository;
            _passwordHashService = passwordHashService;
        }

        public async Task<TokenResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(request.UserName);
            if (user == null)
                throw new AuFrameWorkException("Kullanıcı adı veya şifre hatalı", "INVALID_CREDENTIALS", "ValidationError");

            var isPasswordValid = await _passwordHashService.VerifyPasswordAsync(
                request.Password,
                user.PasswordHash,
                user.PasswordSalt
            );

            if (!isPasswordValid)
                throw new AuFrameWorkException("Kullanıcı adı veya şifre hatalı", "INVALID_CREDENTIALS", "ValidationError");

            if (!user.IsActive)
                throw new AuFrameWorkException("Hesabınız aktif değil", "ACCOUNT_INACTIVE", "ValidationError");

            var token = JwtTokenGenerator.GenerateToken(user);

            user.LastLoginDate = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            // Kullanıcının rollerini al
            var roles = user.Roles?.Where(r => r.IsActive && !r.IsDeleted)
                                 .Select(r => r.Name)
                                 .ToList() ?? new List<string>();

            // Token yanıtına rolleri ekle
            token.Roles = roles;

            return token;
        }
    }
} 