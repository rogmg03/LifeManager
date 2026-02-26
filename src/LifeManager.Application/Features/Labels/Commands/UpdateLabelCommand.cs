using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.Labels.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Labels.Commands;

public record UpdateLabelCommand(
    Guid Id,
    string Name,
    string Color) : IRequest<LabelDto>, IBaseCommand;

public class UpdateLabelCommandHandler : IRequestHandler<UpdateLabelCommand, LabelDto>
{
    private readonly IUnitOfWork _uow;
    public UpdateLabelCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<LabelDto> Handle(UpdateLabelCommand request, CancellationToken ct)
    {
        var label = await _uow.Labels.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Label", request.Id);

        label.Name = request.Name;
        label.Color = request.Color;

        _uow.Labels.Update(label);
        await _uow.SaveChangesAsync(ct);
        return LabelDto.FromEntity(label);
    }
}
