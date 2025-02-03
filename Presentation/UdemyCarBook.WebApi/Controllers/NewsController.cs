using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UdemyCarBook.Application.Features.Mediator.Commands.NewsCommands;
using UdemyCarBook.Application.Features.Mediator.Queries.NewsQueries;
using UdemyCarBook.Application.Interfaces.IService;
using UdemyCarBook.Domain.Exceptions;

namespace UdemyCarBook.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPermissionAuthorizationService _permissionAuthorizationService;
        private readonly ILogService _logService;
        private readonly IUserService _userService;

        public NewsController(
            IMediator mediator,
            IPermissionAuthorizationService permissionAuthorizationService,
            ILogService logService,
            IUserService userService)
        {
            _mediator = mediator;
            _permissionAuthorizationService = permissionAuthorizationService;
            _logService = logService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _mediator.Send(new GetNewsQuery());
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "GetAllNews",
                    "Haberler listelenirken hata oluştu"
                );
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetNewsByIdQuery(id));
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "GetNewsById",
                    "Haber detayı görüntülenirken hata oluştu"
                );
                throw;
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateNewsCommand command)
        {
            try
            {
                var userId = await _userService.GetCurrentUserIdAsync();
                if (!await _permissionAuthorizationService.HasPermissionAsync(userId, "NEWS_CREATE"))
                    return Forbid();

                await _mediator.Send(command);
                return Ok("Haber başarıyla oluşturuldu");
            }
            catch (AuFrameWorkException ex)
            {
                await _logService.CreateLog(
                    "Hata",
                    $"Haber oluşturulurken hata: {ex.Message}",
                    "Error",
                    "News"
                );
                return StatusCode(500, new { ErrorCode = ex.ErrorCode, Message = ex.Message });
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateNewsCommand command)
        {
            try
            {
                var userId = await _userService.GetCurrentUserIdAsync();
                if (!await _permissionAuthorizationService.HasPermissionAsync(userId, "NEWS_UPDATE"))
                    return Forbid();

                await _mediator.Send(command);
                return Ok("Haber başarıyla güncellendi");
            }
            catch (AuFrameWorkException ex)
            {
                await _logService.CreateLog(
                    "Hata",
                    $"Haber güncellenirken hata: {ex.Message}",
                    "Error",
                    "News"
                );
                return StatusCode(500, new { ErrorCode = ex.ErrorCode, Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var userId = await _userService.GetCurrentUserIdAsync();
                if (!await _permissionAuthorizationService.HasPermissionAsync(userId, "NEWS_DELETE"))
                    return Forbid();

                var command = new SoftDeleteNewsCommand { Id = id };
                await _mediator.Send(command);
                return Ok("Haber başarıyla silindi");
            }
            catch (AuFrameWorkException ex)
            {
                await _logService.CreateLog(
                    "Hata",
                    $"Haber silinirken hata: {ex.Message}",
                    "Error",
                    "News"
                );
                return StatusCode(500, new { ErrorCode = ex.ErrorCode, Message = ex.Message });
            }
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(Guid categoryId)
        {
            try
            {
                var result = await _mediator.Send(new GetNewsByCategoryQuery(categoryId));
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "GetNewsByCategory",
                    "Kategoriye göre haberler listelenirken hata oluştu"
                );
                throw;
            }
        }

        [HttpGet("author/{authorId}")]
        public async Task<IActionResult> GetByAuthor(Guid authorId)
        {
            try
            {
                var result = await _mediator.Send(new GetNewsByAuthorQuery(authorId));
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "GetNewsByAuthor",
                    "Yazara göre haberler listelenirken hata oluştu"
                );
                throw;
            }
        }

        [HttpGet("tag/{tagId}")]
        public async Task<IActionResult> GetByTag(Guid tagId)
        {
            try
            {
                var result = await _mediator.Send(new GetNewsByTagQuery(tagId));
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "GetNewsByTag",
                    "Etikete göre haberler listelenirken hata oluştu"
                );
                throw;
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] SearchNewsQuery query)
        {
            try
            {
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "SearchNews",
                    "Haber araması yapılırken hata oluştu"
                );
                throw;
            }
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatest([FromQuery] int count = 5)
        {
            try
            {
                var result = await _mediator.Send(new GetLatestNewsQuery(count));
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "GetLatestNews",
                    "Son haberler listelenirken hata oluştu"
                );
                throw;
            }
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopular([FromQuery] int count = 5)
        {
            try
            {
                var result = await _mediator.Send(new GetPopularNewsQuery(count));
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "GetPopularNews",
                    "Popüler haberler listelenirken hata oluştu"
                );
                throw;
            }
        }
    }
} 