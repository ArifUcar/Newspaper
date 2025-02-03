using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Commands.UserCommands;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Application.Interfaces.IService;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Domain.Exceptions;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.UserHandlers.WriteUserHandlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUserRepository _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;
        private readonly IRoleRepository _roleRepository;
        private readonly IPasswordHashService _passwordHashService;

        public UpdateUserCommandHandler(
            IUserRepository repository,
            IHistoryService historyService,
            ILogService logService,
            IRoleRepository roleRepository,
            IPasswordHashService passwordHashService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
            _roleRepository = roleRepository;
            _passwordHashService = passwordHashService;
        }

        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _repository.GetByIdWithDetailsAsync(request.Id);
                if (user == null)
                    throw new AuFrameWorkException("Kullanıcı bulunamadı", "USER_NOT_FOUND", "NotFound");

                if (string.IsNullOrEmpty(request.UserName))
                    throw new AuFrameWorkException("Kullanıcı adı boş olamaz", "USERNAME_REQUIRED", "ValidationError");

                if (string.IsNullOrEmpty(request.Email))
                    throw new AuFrameWorkException("E-posta adresi boş olamaz", "EMAIL_REQUIRED", "ValidationError");

                // Kullanıcı adı değişmişse kontrol et
                if (request.UserName != user.UserName)
                {
                    var existingUser = await _repository.GetByUsernameAsync(request.UserName);
                    if (existingUser != null && existingUser.Id != request.Id)
                        throw new AuFrameWorkException("Bu kullanıcı adı zaten kullanılıyor", "USER_EXISTS", "ValidationError");
                }

                // Email değişmişse kontrol et
                if (request.Email != user.Email)
                {
                    var existingUserByEmail = await _repository.GetByEmailAsync(request.Email);
                    if (existingUserByEmail != null && existingUserByEmail.Id != request.Id)
                        throw new AuFrameWorkException("Bu e-posta adresi zaten kullanılıyor", "EMAIL_EXISTS", "ValidationError");
                }

                // Şifre değiştirilmek isteniyorsa
                if (!string.IsNullOrEmpty(request.Password))
                {
                    var (passwordHash, passwordSalt) = await _passwordHashService.HashPasswordAsync(request.Password);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    user.Password = null; // Plain text şifreyi temizle
                }

                user.UserName = request.UserName?.Trim();
                user.Email = request.Email?.Trim().ToLower();
                user.FirstName = request.FirstName?.Trim();
                user.LastName = request.LastName?.Trim();
                user.PhoneNumber = request.PhoneNumber?.Trim();
                user.UserType = request.UserType;
                user.IsActive = request.IsActive;
                user.LastModifiedDate = DateTime.UtcNow;

                // Rolleri güncelle
                if (request.Roles != null)
                {
                    user.Roles.Clear(); // Mevcut rolleri temizle
                    foreach (var roleName in request.Roles)
                    {
                        if (string.IsNullOrEmpty(roleName)) continue;

                        var role = await _roleRepository.GetByNameAsync(roleName.Trim());
                        if (role != null)
                        {
                            user.Roles.Add(role);
                        }
                    }
                }

                await _repository.UpdateAsync(user);
                await _historyService.SaveHistory(user, "Update");

                await _logService.CreateLog(
                    "Kullanıcı Güncelleme",
                    $"'{request.UserName}' kullanıcı adlı kullanıcı güncellendi",
                    "Update",
                    "User"
                );
            }
            catch (Exception ex)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "UserUpdate",
                    $"Kullanıcı güncellenirken hata: {ex.Message} - {request.UserName}"
                );
                throw new AuFrameWorkException(
                    $"Kullanıcı güncellenirken bir hata oluştu: {ex.Message}",
                    "UPDATE_ERROR",
                    "Error"
                );
            }
        }
    }
} 