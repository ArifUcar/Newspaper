using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Queries.UserQueries;
using UdemyCarBook.Application.Features.Mediator.Results.UserResults;
using UdemyCarBook.Application.Interfaces;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.UserHandlers.ReadUserHandlers
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, List<GetUserQueryResult>>
    {
        private readonly IUserRepository _repository;

        public GetUserQueryHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GetUserQueryResult>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var users = await _repository.GetAllWithDetailsAsync();

            return users.Select(x => new GetUserQueryResult
            {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                PhoneNumber = x.PhoneNumber,
                UserType = x.UserType,
                IsActive = x.IsActive,
                Roles = x.Roles?.Select(r => r.Name).ToList() ?? new List<string>(),
                NewsCount = x.CreatedNews?.Count(n => !n.IsDeleted) ?? 0,
                CommentsCount = x.CreatedComments?.Count(c => !c.IsDeleted) ?? 0,
                CreatedDate = x.CreatedDate,
                CreatedByUserName = x.CreatedByUser?.UserName
            }).ToList();
        }
    }
} 