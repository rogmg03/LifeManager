using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.WorkInitiativeDetails.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.WorkInitiativeDetails.Commands;

public record UpdateWorkInitiativeDetailCommand(
    Guid ProjectId,
    string? ClientName,
    decimal? ContractValue,
    string? BillingType,
    decimal? HourlyRate,
    decimal? EstimatedHours,
    decimal? LoggedHours,
    bool IsInternal) : IRequest<WorkInitiativeDetailDto>, IBaseCommand;

public class UpdateWorkInitiativeDetailCommandHandler
    : IRequestHandler<UpdateWorkInitiativeDetailCommand, WorkInitiativeDetailDto>
{
    private readonly IUnitOfWork _uow;
    public UpdateWorkInitiativeDetailCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<WorkInitiativeDetailDto> Handle(UpdateWorkInitiativeDetailCommand request, CancellationToken ct)
    {
        var detail = await _uow.WorkInitiativeDetails.GetByProjectIdAsync(request.ProjectId, ct)
            ?? throw new NotFoundException("WorkInitiativeDetail", request.ProjectId);

        detail.ClientName = request.ClientName;
        detail.ContractValue = request.ContractValue;
        detail.BillingType = request.BillingType;
        detail.HourlyRate = request.HourlyRate;
        detail.EstimatedHours = request.EstimatedHours;
        detail.LoggedHours = request.LoggedHours;
        detail.IsInternal = request.IsInternal;

        _uow.WorkInitiativeDetails.Update(detail);
        await _uow.SaveChangesAsync(ct);
        return WorkInitiativeDetailDto.FromEntity(detail);
    }
}
