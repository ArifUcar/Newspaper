using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Commands.NewsletterCommands;
using UdemyCarBook.Application.Features.Mediator.Queries.NewsletterQueries;

namespace UdemyCarBook.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewslettersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NewslettersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var values = await _mediator.Send(new GetNewsletterQuery());
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var value = await _mediator.Send(new GetNewsletterByIdQuery { Id = id });
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateNewsletterCommand command)
        {
            await _mediator.Send(command);
            return Ok("Bülten aboneliği başarıyla oluşturuldu");
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateNewsletterCommand command)
        {
            await _mediator.Send(command);
            return Ok("Bülten aboneliği başarıyla güncellendi");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            await _mediator.Send(new RemoveNewsletterCommand { Id = id });
            return Ok("Bülten aboneliği başarıyla silindi");
        }

        [HttpDelete("soft-delete/{id}")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            await _mediator.Send(new SoftDeleteNewsletterCommand { Id = id });
            return Ok("Bülten aboneliği başarıyla arşivlendi");
        }
    }
} 