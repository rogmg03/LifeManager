using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Labels.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Labels.Commands;

public record CreateLabelCommand(
    string Name,
    string Color) : IRequest<LabelDto>, IBaseCommand;

public class CreateLabelCommandHandler : IRequestHandler<CreateLabelCommand, LabelDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public CreateLabelCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    { _uow = uow; _currentUser = currentUser; }

    public async Task<LabelDto> Handle(CreateLabelCommand request, CancellationToken ct)
    {
        var label = new Label
        {
            UserId = _currentUser.UserId,
            Name = request.Name,
            Color = request.Color
        };
        await _uow.Labels.AddAsync(label, ct);
        await _uow.SaveChangesAsync(ct);
        return LabelDto.FromEntity(label);
    }
}
