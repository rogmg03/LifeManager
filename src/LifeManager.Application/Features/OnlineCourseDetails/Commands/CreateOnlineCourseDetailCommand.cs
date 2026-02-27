using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.OnlineCourseDetails.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.OnlineCourseDetails.Commands;

public record CreateOnlineCourseDetailCommand(
    Guid ProjectId,
    string? Platform,
    string? CourseUrl,
    string? InstructorName,
    int? TotalLessons,
    int? CompletedLessons,
    string? CertificateUrl,
    DateOnly? StartedAt,
    DateOnly? CompletedAt) : IRequest<OnlineCourseDetailDto>, IBaseCommand;

public class CreateOnlineCourseDetailCommandHandler
    : IRequestHandler<CreateOnlineCourseDetailCommand, OnlineCourseDetailDto>
{
    private readonly IUnitOfWork _uow;
    public CreateOnlineCourseDetailCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<OnlineCourseDetailDto> Handle(CreateOnlineCourseDetailCommand request, CancellationToken ct)
    {
        var project = await _uow.Projects.GetByIdAsync(request.ProjectId, ct)
            ?? throw new NotFoundException("Project", request.ProjectId);

        if (project.Type != ProjectType.OnlineCourse)
            throw new BadRequestException("Project is not an Online Course type.");

        var existing = await _uow.OnlineCourseDetails.GetByProjectIdAsync(request.ProjectId, ct);
        if (existing is not null)
            throw new ConflictException("Online course detail already exists for this project.");

        var detail = new OnlineCourseDetail
        {
            ProjectId = request.ProjectId,
            Platform = request.Platform,
            CourseUrl = request.CourseUrl,
            InstructorName = request.InstructorName,
            TotalLessons = request.TotalLessons,
            CompletedLessons = request.CompletedLessons,
            CertificateUrl = request.CertificateUrl,
            StartedAt = request.StartedAt,
            CompletedAt = request.CompletedAt
        };

        await _uow.OnlineCourseDetails.AddAsync(detail, ct);
        await _uow.SaveChangesAsync(ct);
        return OnlineCourseDetailDto.FromEntity(detail);
    }
}
