using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;

namespace LifeManager.Application.Features.AssignmentDetails.DTOs;

public record AssignmentDetailDto(
    Guid TaskId,
    AssignmentType AssignmentType,
    decimal? Grade,
    string? GradeLetter,
    decimal? Weight,
    string? SubmissionLink)
{
    public static AssignmentDetailDto FromEntity(AssignmentDetail d) => new(
        d.TaskId,
        d.AssignmentType,
        d.Grade,
        d.GradeLetter,
        d.Weight,
        d.SubmissionLink);
}
