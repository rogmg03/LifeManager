using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Activity.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Activity.Commands;

public record CreateActivityEntryCommand(
    Guid ProjectId,
    string Description) : IRequest<ActivityEntryDto>, IBaseCommand;

public class CreateActivityEntryCommandHandler
    : IRequestHandler<CreateActivityEntryCommand, ActivityEntryDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public CreateActivityEntryCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    { _uow = uow; _currentUser = currentUser; }

    public async Task<ActivityEntryDto> Handle(
        CreateActivityEntryCommand request, CancellationToken ct)
    {
        _ = await _uow.Projects.GetByIdAsync(request.ProjectId, ct)
            ?? throw new NotFoundException("Project", request.ProjectId);

        var entry = new ActivityEntry
        {
            UserId = _currentUser.UserId,
            ProjectId = request.ProjectId,
            ActivityType = ActivityType.ManualEntry,
            Description = request.Description
        };

        await _uow.ActivityEntries.AddAsync(entry, ct);
        await _uow.SaveChangesAsync(ct);
        return ActivityEntryDto.FromEntity(entry);
    }
}
