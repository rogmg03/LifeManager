using LifeManager.Domain.Enums;

namespace LifeManager.Application.Features.AssignmentDetails.DTOs;

public record UpsertAssignmentDetailRequest(
    AssignmentType AssignmentType,
    decimal? Grade,
    string? GradeLetter,
    decimal? Weight,
    string? SubmissionLink);
