using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyCarBook.Application.Features.Mediator.Queries.AboutQueries;
using UdemyCarBook.Application.Features.Mediator.Results.AboutResults;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Application.Interfaces.IService;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Domain.Exceptions;

namespace UdemyCarBook.Application.Features.Mediator.Handlers.AboutHandlers.ReadAboutHandlers
{
    public class GetAboutQueryHandler : IRequestHandler<GetAboutQuery, List<GetAboutQueryResult>>
    {
        private readonly IRepository<About> _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public GetAboutQueryHandler(IRepository<About> repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task<List<GetAboutQueryResult>> Handle(GetAboutQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _repository.GetAllAsync();
                
                foreach(var value in values)
                {
                    await _historyService.SaveHistory(value, "List");
                }

                await _logService.CreateLog(
                    "About Listesi",
                    $"Toplam {values.Count} about kaydı listelendi",
                    "List",
                    "About"
                );

                return values.Select(x => new GetAboutQueryResult
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    ImageUrl = x.ImageUrl
                }).ToList();
            }
            catch (Exception ex)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "AboutGetAll",
                    "About listesi alınırken hata oluştu"
                );
                throw new AuFrameWorkException(
                    "About listesi alınırken bir hata oluştu", 
                    "LIST_ERROR",
                    "Error"
                );
            }
        }
    }
}
