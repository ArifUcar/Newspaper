using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Commands.ContactCommands;
using UdemyCarBook.Application.Features.Mediator.Queries.ContactQueries;


namespace UdemyCarBook.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContactsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var values = await _mediator.Send(new GetContactQuery());
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var value = await _mediator.Send(new GetContactByIdQuery { Id = id });
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateContactCommand command)
        {
            await _mediator.Send(command);
            return Ok("İletişim mesajı başarıyla oluşturuldu");
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateContactCommand command)
        {
            await _mediator.Send(command);
            return Ok("İletişim mesajı başarıyla güncellendi");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            await _mediator.Send(new RemoveContactCommand { Id = id });
            return Ok("İletişim mesajı başarıyla silindi");
        }

        [HttpDelete("soft-delete/{id}")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            await _mediator.Send(new SoftDeleteContactCommand { Id = id });
            return Ok("İletişim mesajı başarıyla arşivlendi");
        }
    }
} 