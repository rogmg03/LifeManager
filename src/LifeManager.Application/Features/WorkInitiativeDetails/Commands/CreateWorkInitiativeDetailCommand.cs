using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.WorkInitiativeDetails.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.WorkInitiativeDetails.Commands;

public record CreateWorkInitiativeDetailCommand(
    Guid ProjectId,
    string? ClientName,
    decimal? ContractValue,
    string? BillingType,
    decimal? HourlyRate,
    decimal? EstimatedHours,
    decimal? LoggedHours,
    bool IsInternal) : IRequest<WorkInitiativeDetailDto>, IBaseCommand;

public class CreateWorkInitiativeDetailCommandHandler
    : IRequestHandler<CreateWorkInitiativeDetailCommand, WorkInitiativeDetailDto>
{
    private readonly IUnitOfWork _uow;
    public CreateWorkInitiativeDetailCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<WorkInitiativeDetailDto> Handle(CreateWorkInitiativeDetailCommand request, CancellationToken ct)
    {
        var project = await _uow.Projects.GetByIdAsync(request.ProjectId, ct)
            ?? throw new NotFoundException("Project", request.ProjectId);

        if (project.Type != ProjectType.WorkInitiative)
            throw new BadRequestException("Project is not a Work Initiative type.");

        var existing = await _uow.WorkInitiativeDetails.GetByProjectIdAsync(request.ProjectId, ct);
        if (existing is not null)
            throw new ConflictException("Work initiative detail already exists for this project.");

        var detail = new WorkInitiativeDetail
        {
            ProjectId = request.ProjectId,
            ClientName = request.ClientName,
            ContractValue = request.ContractValue,
            BillingType = request.BillingType,
            HourlyRate = request.HourlyRate,
            EstimatedHours = request.EstimatedHours,
            LoggedHours = request.LoggedHours,
            IsInternal = request.IsInternal
        };

        await _uow.WorkInitiativeDetails.AddAsync(detail, ct);
        await _uow.SaveChangesAsync(ct);
        return WorkInitiativeDetailDto.FromEntity(detail);
    }
}
