using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Queries.RoleQueries;
using UdemyCarBook.Application.Features.Mediator.Results.RoleResults;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Domain.Exceptions;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.RoleHandlers.ReadRoleHandlers
{
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, GetRoleByIdQueryResult>
    {
        private readonly IRoleRepository _repository;

        public GetRoleByIdQueryHandler(IRoleRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetRoleByIdQueryResult> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _repository.GetByIdWithDetailsAsync(request.Id);

            if (role == null)
                throw new AuFrameWorkException("Rol bulunamadı", "ROLE_NOT_FOUND", "NotFound");

            return new GetRoleByIdQueryResult
            {
                Id = role.Id,
                Name = role.Name,
                Users = role.Users?.Select(u => new UserInRoleDto
                {
                    Id = u.Id,
                    Username = u.UserName,
                    Email = u.Email
                }).ToList(),
                CreatedDate = role.CreatedDate,
                CreatedByUserName = role.CreatedByUser != null ? role.CreatedByUser.UserName : null,
 
            };
        }
    }
} 