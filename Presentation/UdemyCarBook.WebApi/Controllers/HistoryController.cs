using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UdemyCarBook.Application.Interfaces.IService;
using UdemyCarBook.Domain.Base;

namespace UdemyCarBook.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService _historyService;
        public HistoryController(IHistoryService historyService)
        {
            _historyService = historyService;
        }
        [HttpGet("GetEntityHistory/{entityName}/{entityId}")]
        public async Task<IActionResult> GetEntityHistory(string entityName, Guid entityId)
        {
            var type = Type.GetType($"UdemyCarBook.Domain.Entities.{entityName}");
            if (type == null)
                return BadRequest("Geçersiz entity adı");

            var method = typeof(IHistoryService).GetMethod("GetHistory")?.MakeGenericMethod(type);
            var result = await (Task<List<BaseHistory>>)method.Invoke(_historyService, new object[] { entityId });
            return Ok(result);
        }
        [HttpGet("GetHistoryByEntityName/{entityName}")]
        public async Task<IActionResult> GetHistoryByEntityName(string entityName)
        {
            var history = await _historyService.GetHistoryByEntityName(entityName);
            return Ok(history);
        }
    }
}
