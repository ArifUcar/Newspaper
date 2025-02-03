using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UdemyCarBook.Application.Features.Mediator.Commands.AboutCommands;
using UdemyCarBook.Application.Features.Mediator.Queries.AboutQueries;

namespace UdemyCarBook.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AboutController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> AboutList()
        {
            var values = await _mediator.Send(new GetAboutQuery());
            return Ok(values);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAbout(Guid id)
        {
            var values = await _mediator.Send(new GetAboutByIdQuery(id));
            return Ok(values);
        }
        [HttpPost]
        public async Task<IActionResult> CreateFooterAddres(CreateAboutCommand command)
        {
            await _mediator.Send(command);
            return Ok("FooterAddres başarıyla eklendi");

        }
        [HttpPut]
        public async Task<IActionResult> UpdateFooterAddress(UpdateAboutCommand command)
        {
            await _mediator.Send(command);
            return Ok("FooterAddress başarıyla güncellendi");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteFooterAddress(RemoveAboutCommand command)
        {
            await _mediator.Send(command);
            return Ok("FooterAddress başarıyla silindi");
        }
        [HttpDelete("SoftDelete/{id}")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            await _mediator.Send(new SoftDeleteAboutCommand { Id = id });
            return Ok("Kayıt başarıyla soft delete edildi");
        }
    }
}
