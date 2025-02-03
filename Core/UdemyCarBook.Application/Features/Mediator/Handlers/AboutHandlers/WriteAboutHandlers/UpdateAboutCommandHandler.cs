using MediatR;
using UdemyCarBook.Application.Features.Mediator.Commands.AboutCommands;
using UdemyCarBook.Application.Interfaces;
using UdemyCarBook.Application.Interfaces.IService;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Domain.Exceptions;

public class UpdateAboutCommandHandler : IRequestHandler<UpdateAboutCommand>
{
    private readonly IRepository<About> _repository;
    private readonly IHistoryService _historyService;
    private readonly ILogService _logService;

    public UpdateAboutCommandHandler(IRepository<About> repository, IHistoryService historyService, ILogService logService)
    {
        _repository = repository;
        _historyService = historyService;
        _logService = logService;
    }

    public async Task Handle(UpdateAboutCommand request, CancellationToken cancellationToken)
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

            if (string.IsNullOrEmpty(request.Title))
                throw new AuFrameWorkException("Başlık boş olamaz", "TITLE_REQUIRED", "ValidationError");

            await _historyService.SaveHistory(value, "BeforeUpdate");

            value.Title = request.Title;
            value.Description = request.Description;
            value.ImageUrl = request.ImageUrl;

            await _repository.UpdateAsync(value);
            await _historyService.SaveHistory(value, "AfterUpdate");

            await _logService.CreateLog(
                "About Güncelleme",
                $"ID: {request.Id} olan about güncellendi. Yeni başlık: {request.Title}",
                "Update",
                "About"
            );
        }
        catch (Exception ex) when (ex is not AuFrameWorkException)
        {
            await _logService.CreateErrorLog(
                ex,
                "AboutUpdate",
                $"About güncellenirken hata. ID: {request.Id}"
            );
            throw new AuFrameWorkException(
                "About güncellenirken bir hata oluştu", 
                "UPDATE_ERROR",
                "Error"
            );
        }
    }
}