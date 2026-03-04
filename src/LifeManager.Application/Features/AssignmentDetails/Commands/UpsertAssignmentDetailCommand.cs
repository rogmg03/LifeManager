using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.AssignmentDetails.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.AssignmentDetails.Commands;

public record UpsertAssignmentDetailCommand(
    Guid TaskId,
    AssignmentType AssignmentType,
    decimal? Grade,
    string? GradeLetter,
    decimal? Weight,
    string? SubmissionLink) : IRequest<AssignmentDetailDto>, IBaseCommand;

public class UpsertAssignmentDetailCommandHandler
    : IRequestHandler<UpsertAssignmentDetailCommand, AssignmentDetailDto>
{
    private readonly IUnitOfWork _uow;
    public UpsertAssignmentDetailCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<AssignmentDetailDto> Handle(UpsertAssignmentDetailCommand request, CancellationToken ct)
    {
        var task = await _uow.Tasks.GetByIdAsync(request.TaskId, ct)
            ?? throw new NotFoundException("Task", request.TaskId);

        var project = await _uow.Projects.GetByIdAsync(task.ProjectId, ct)
            ?? throw new NotFoundException("Project", task.ProjectId);

        if (project.Type != ProjectType.CollegeCourse)
            throw new BadRequestException("Task does not belong to a College Course project.");

        var existing = await _uow.AssignmentDetails.GetByTaskIdAsync(request.TaskId, ct);

        if (existing is null)
        {
            var detail = new AssignmentDetail
            {
                TaskId = request.TaskId,
                AssignmentType = request.AssignmentType,
                Grade = request.Grade,
                GradeLetter = request.GradeLetter,
                Weight = request.Weight,
                SubmissionLink = request.SubmissionLink
            };
            await _uow.AssignmentDetails.AddAsync(detail, ct);
            await _uow.SaveChangesAsync(ct);
            return AssignmentDetailDto.FromEntity(detail);
        }

        existing.AssignmentType = request.AssignmentType;
        existing.Grade = request.Grade;
        existing.GradeLetter = request.GradeLetter;
        existing.Weight = request.Weight;
        existing.SubmissionLink = request.SubmissionLink;
        _uow.AssignmentDetails.Update(existing);
        await _uow.SaveChangesAsync(ct);
        return AssignmentDetailDto.FromEntity(existing);
    }
}
