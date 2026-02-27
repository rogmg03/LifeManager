using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.FreeTimeRatios.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.FreeTimeRatios.Commands;

public record UpdateFreeTimeRatioCommand(decimal WorkMinutesPerFreeMinute) : IRequest<FreeTimeRatioDto>, IBaseCommand;

public class UpdateFreeTimeRatioCommandHandler : IRequestHandler<UpdateFreeTimeRatioCommand, FreeTimeRatioDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public UpdateFreeTimeRatioCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<FreeTimeRatioDto> Handle(UpdateFreeTimeRatioCommand request, CancellationToken ct)
    {
        var ratio = await _uow.FreeTimeRatios.GetByUserIdAsync(_currentUser.UserId, ct);

        if (ratio is null)
        {
            ratio = new FreeTimeRatio
            {
                UserId = _currentUser.UserId,
                WorkMinutesPerFreeMinute = request.WorkMinutesPerFreeMinute
            };
            await _uow.FreeTimeRatios.AddAsync(ratio, ct);
        }
        else
        {
            ratio.WorkMinutesPerFreeMinute = request.WorkMinutesPerFreeMinute;
            _uow.FreeTimeRatios.Update(ratio);
        }

        await _uow.SaveChangesAsync(ct);
        return FreeTimeRatioDto.FromEntity(ratio);
    }
}
