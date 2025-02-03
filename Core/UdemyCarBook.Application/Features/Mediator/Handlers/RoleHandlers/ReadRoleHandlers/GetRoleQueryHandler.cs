using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Queries.RoleQueries;
using UdemyCarBook.Application.Features.Mediator.Results.RoleResults;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Domain.Entities;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.RoleHandlers.ReadRoleHandlers
{
    public class GetRoleQueryHandler : IRequestHandler<GetRoleQuery, List<GetRoleQueryResult>>
    {
        private readonly IRoleRepository _repository;

        public GetRoleQueryHandler(IRoleRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GetRoleQueryResult>> Handle(GetRoleQuery request, CancellationToken cancellationToken)
        {
            var roles = await _repository.GetAllWithDetailsAsync();

            return roles.Select(x => new GetRoleQueryResult
            {
                Id = x.Id,
                Name = x.Name,
                UserCount = x.Users?.Count ?? 0,
                CreatedDate = x.CreatedDate,
                CreatedByUserName = x.CreatedByUser != null ? x.CreatedByUser.UserName : null
            }).ToList();
        }
    }
} 