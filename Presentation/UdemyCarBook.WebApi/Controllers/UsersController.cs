using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UdemyCarBook.Application.Features.Mediator.Commands.UserCommands;
using UdemyCarBook.Application.Features.Mediator.Queries.UserQueries;
using UdemyCarBook.Application.Interfaces.IService;

namespace UdemyCarBook.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPermissionAuthorizationService _permissionAuthorizationService;
        private readonly ILogService _logService;

        public UsersController(
            IMediator mediator, 
            IPermissionAuthorizationService permissionAuthorizationService,
            ILogService logService)
        {
            _mediator = mediator;
            _permissionAuthorizationService = permissionAuthorizationService;
            _logService = logService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            try 
            {
                // Kullanıcı bilgilerini logla
                var claims = User.Claims.Select(c => $"{c.Type}: {c.Value}");
                await _logService.CreateLog(
                    "Yetki Kontrolü",
                    $"Kullanıcı Claims: {string.Join(", ", claims)}",
                    "Debug",
                    "Authorization"
                );

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
                {
                    await _logService.CreateLog(
                        "Yetki Hatası",
                        "Geçersiz kullanıcı ID",
                        "Error",
                        "Authorization"
                    );
                    return Unauthorized();
                }

                // İzin kontrolünü logla
                var hasPermission = await _permissionAuthorizationService.HasPermissionAsync(userGuid, "USR_VIEW_ALL");
                await _logService.CreateLog(
                    "İzin Kontrolü",
                    $"Kullanıcı ID: {userGuid}, İzin: USR_VIEW_ALL, Sonuç: {hasPermission}",
                    "Debug",
                    "Authorization"
                );

                if (!hasPermission)
                {
                    return Forbid();
                }

                var result = await _mediator.Send(new GetUserQuery());
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "GetAllUsers",
                    "Kullanıcılar listelenirken hata oluştu"
                );
                throw;
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
                {
                    await _logService.CreateLog(
                        "Yetki Hatası",
                        "Geçersiz kullanıcı ID",
                        "Error",
                        "Authorization"
                    );
                    return Unauthorized();
                }

                var hasPermission = await _permissionAuthorizationService.HasPermissionAsync(userGuid, "USR_VIEW_ALL");
                await _logService.CreateLog(
                    "İzin Kontrolü",
                    $"Kullanıcı ID: {userGuid}, İzin: USR_VIEW_ALL, Sonuç: {hasPermission}",
                    "Debug",
                    "Authorization"
                );

                if (!hasPermission)
                {
                    return Forbid();
                }

                var result = await _mediator.Send(new GetUserByIdQuery(id));
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "GetUserById",
                    "Kullanıcı detayı görüntülenirken hata oluştu"
                );
                throw;
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Update([FromBody] UpdateUserCommand command)
        {
            await _mediator.Send(command);
            return Ok("Kullanıcı başarıyla güncellendi");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new SoftDeleteUserCommand(id));
            return Ok("Kullanıcı başarıyla silindi");
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
} 