using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.OnlineCourseDetails.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.OnlineCourseDetails.Commands;

public record UpdateOnlineCourseDetailCommand(
    Guid ProjectId,
    string? Platform,
    string? CourseUrl,
    string? InstructorName,
    int? TotalLessons,
    int? CompletedLessons,
    string? CertificateUrl,
    DateOnly? StartedAt,
    DateOnly? CompletedAt) : IRequest<OnlineCourseDetailDto>, IBaseCommand;

public class UpdateOnlineCourseDetailCommandHandler
    : IRequestHandler<UpdateOnlineCourseDetailCommand, OnlineCourseDetailDto>
{
    private readonly IUnitOfWork _uow;
    public UpdateOnlineCourseDetailCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<OnlineCourseDetailDto> Handle(UpdateOnlineCourseDetailCommand request, CancellationToken ct)
    {
        var detail = await _uow.OnlineCourseDetails.GetByProjectIdAsync(request.ProjectId, ct)
            ?? throw new NotFoundException("OnlineCourseDetail", request.ProjectId);

        detail.Platform = request.Platform;
        detail.CourseUrl = request.CourseUrl;
        detail.InstructorName = request.InstructorName;
        detail.TotalLessons = request.TotalLessons;
        detail.CompletedLessons = request.CompletedLessons;
        detail.CertificateUrl = request.CertificateUrl;
        detail.StartedAt = request.StartedAt;
        detail.CompletedAt = request.CompletedAt;

        _uow.OnlineCourseDetails.Update(detail);
        await _uow.SaveChangesAsync(ct);
        return OnlineCourseDetailDto.FromEntity(detail);
    }
}
