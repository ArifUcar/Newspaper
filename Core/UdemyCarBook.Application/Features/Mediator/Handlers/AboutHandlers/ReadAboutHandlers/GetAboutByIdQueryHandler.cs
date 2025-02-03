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
    public class GetAboutByIdQueryHandler : IRequestHandler<GetAboutByIdQuery, GetAboutByIdQueryResult>
    {
        private readonly IRepository<About> _repository;
        private readonly IHistoryService _historyService;
        private readonly ILogService _logService;

        public GetAboutByIdQueryHandler(IRepository<About> repository, IHistoryService historyService, ILogService logService)
        {
            _repository = repository;
            _historyService = historyService;
            _logService = logService;
        }

        public async Task<GetAboutByIdQueryResult> Handle(GetAboutByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var value = await _repository.GetByIdAsync(request.Id);
                if (value == null)
                {
                    throw new AuFrameWorkException(
                        $"ID: {request.Id} olan about bulunamadı", 
                        "RECORD_NOT_FOUND",
                        "NotFound"
                    );
                }

                await _historyService.SaveHistory(value, "Read");
                await _logService.CreateLog(
                    "About Görüntüleme",
                    $"ID: {request.Id} olan about görüntülendi",
                    "Read",
                    "About"
                );

                return new GetAboutByIdQueryResult
                {
                    Id = value.Id,
                    Title = value.Title,
                    Description = value.Description,
                    ImageUrl = value.ImageUrl
                };
            }
            catch (Exception ex) when (ex is not AuFrameWorkException)
            {
                await _logService.CreateErrorLog(
                    ex,
                    "AboutGetById",
                    $"About görüntülenirken hata. ID: {request.Id}"
                );
                throw new AuFrameWorkException(
                    "About bilgisi alınırken bir hata oluştu", 
                    "READ_ERROR",
                    "Error"
                );
            }
        }
    }
}
