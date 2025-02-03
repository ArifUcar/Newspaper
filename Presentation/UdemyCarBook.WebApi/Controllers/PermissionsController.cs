using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Commands.PermissionCommands;
using UdemyCarBook.Application.Features.Mediator.Queries.PermissionQueries;
using UdemyCarBook.Application.Interfaces.IService;

namespace UdemyCarBook.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPermissionAuthorizationService _authorizationService;
        private readonly IUserService _userService;

        public PermissionsController(
            IMediator mediator,
            IPermissionAuthorizationService authorizationService,
            IUserService userService)
        {
            _mediator = mediator;
            _authorizationService = authorizationService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = await _userService.GetCurrentUserIdAsync();
            if (!await _authorizationService.HasPermissionAsync(userId, "PERMISSION.VIEW"))
                return Forbid();

            var query = new GetPermissionQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePermissionCommand command)
        {
            var userId = await _userService.GetCurrentUserIdAsync();
            if (!await _authorizationService.HasPermissionAsync(userId, "PERMISSION.CREATE"))
                return Forbid();

            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut("{id}/activate")]
        public async Task<IActionResult> Activate(Guid id)
        {
            var userId = await _userService.GetCurrentUserIdAsync();
            if (!await _authorizationService.HasPermissionAsync(userId, "PERMISSION.UPDATE"))
                return Forbid();

            var command = new UpdatePermissionStatusCommand { Id = id, IsActive = true };
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            var userId = await _userService.GetCurrentUserIdAsync();
            if (!await _authorizationService.HasPermissionAsync(userId, "PERMISSION.UPDATE"))
                return Forbid();

            var command = new UpdatePermissionStatusCommand { Id = id, IsActive = false };
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = await _userService.GetCurrentUserIdAsync();
            if (!await _authorizationService.HasPermissionAsync(userId, "PERMISSION.DELETE"))
                return Forbid();

            var command = new DeletePermissionCommand { Id = id };
            await _mediator.Send(command);
            return Ok();
        }
    }
} 