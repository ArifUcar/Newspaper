using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Queries.PermissionQueries;
using UdemyCarBook.Application.Features.Mediator.Results.PermissionResults;
using UdemyCarBook.Application.Interfaces;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.PermissionHandlers.ReadPermissionHandlers
{
    public class GetPermissionQueryHandler : IRequestHandler<GetPermissionQuery, List<GetPermissionQueryResult>>
    {
        private readonly IPermissionRepository _repository;

        public GetPermissionQueryHandler(IPermissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GetPermissionQueryResult>> Handle(GetPermissionQuery request, CancellationToken cancellationToken)
        {
            var permissions = await _repository.GetAllWithDetailsAsync();

            return permissions.Select(x => new GetPermissionQueryResult
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Group = x.Group,
                Code = x.Code,
                IsActive = x.IsActive,
                RoleCount = x.Roles?.Count ?? 0
            }).ToList();
        }
    }
} 