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
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
    {
        private readonly IUserRepository _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;
        private readonly IRoleRepository _roleRepository;
        private readonly IPasswordHashService _passwordHashService;

        public CreateUserCommandHandler(
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

        public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await _repository.Context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                if (string.IsNullOrEmpty(request.UserName))
                    throw new AuFrameWorkException("Kullanıcı adı boş olamaz", "USERNAME_REQUIRED", "ValidationError");

                if (string.IsNullOrEmpty(request.Password))
                    throw new AuFrameWorkException("Şifre boş olamaz", "PASSWORD_REQUIRED", "ValidationError");

                if (string.IsNullOrEmpty(request.Email))
                    throw new AuFrameWorkException("E-posta adresi boş olamaz", "EMAIL_REQUIRED", "ValidationError");

                var existingUserByEmail = await _repository.GetByEmailAsync(request.Email);
                if (existingUserByEmail != null)
                    throw new AuFrameWorkException("Bu e-posta adresi zaten kullanılıyor", "EMAIL_EXISTS", "ValidationError");

                var existingUser = await _repository.GetByUsernameAsync(request.UserName);
                if (existingUser != null)
                    throw new AuFrameWorkException("Bu kullanıcı adı zaten kullanılıyor", "USER_EXISTS", "ValidationError");

                // Şifreyi hashle
                var (passwordHash, passwordSalt) = await _passwordHashService.HashPasswordAsync(request.Password);

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = request.UserName?.Trim(),
                    Password = null,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Email = request.Email?.Trim().ToLower(),
                    FirstName = request.FirstName?.Trim(),
                    LastName = request.LastName?.Trim(),
                    PhoneNumber = request.PhoneNumber?.Trim(),
                    UserType = request.UserType,
                    IsActive = request.IsActive,
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                };

                // Kullanıcıyı oluştur
                await _repository.CreateAsync(user);

                // Rolleri kontrol et ve ekle
                var roleNames = request.Roles?.Where(r => !string.IsNullOrEmpty(r)).ToList() ?? new List<string>();
                if (!roleNames.Any())
                {
                    roleNames.Add("USER"); // Varsayılan rol
                }

                foreach (var roleName in roleNames)
                {
                    var role = await _roleRepository.GetByNameAsync(roleName.Trim().ToUpper());
                    if (role == null)
                    {
                        throw new AuFrameWorkException(
                            $"'{roleName}' rolü sistemde bulunamadı",
                            "ROLE_NOT_FOUND",
                            "ValidationError"
                        );
                    }

                    // UserRole ilişkisini doğrudan ekle
                    var userRole = new UserRole
                    {
                        UserId = user.Id,
                        RoleId = role.Id,
                        CreatedDate = DateTime.UtcNow
                    };

                    _repository.Context.Set<UserRole>().Add(userRole);

                    await _logService.CreateLog(
                        "Rol Ataması",
                        $"'{user.UserName}' kullanıcısına '{role.Name}' rolü atandı",
                        "Create",
                        "UserRole"
                    );
                }

                await _repository.Context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                await _historyService.SaveHistory(user, "Create");
                
                await _logService.CreateLog(
                    "Kullanıcı Oluşturma",
                    $"'{request.UserName}' kullanıcı adlı kullanıcı oluşturuldu",
                    "Create",
                    "User"
                );
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                await _logService.CreateErrorLog(
                    ex,
                    "UserCreate",
                    $"Kullanıcı oluşturulurken hata: {ex.Message} - {request.UserName}"
                );
                throw new AuFrameWorkException(
                    $"Kullanıcı oluşturulurken bir hata oluştu: {ex.Message}", 
                    "CREATE_ERROR",
                    "Error"
                );
            }
        }
    }
} 