using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.CollegeCourseDetails.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.CollegeCourseDetails.Commands;

public record UpdateCollegeCourseDetailCommand(
    Guid ProjectId,
    string? InstitutionName,
    string? CourseName,
    string? CourseCode,
    string? Semester,
    int? Year,
    decimal? Credits,
    string? Professor,
    string? Room,
    string? Schedule,
    decimal? CurrentGrade,
    decimal? TargetGrade) : IRequest<CollegeCourseDetailDto>, IBaseCommand;

public class UpdateCollegeCourseDetailCommandHandler
    : IRequestHandler<UpdateCollegeCourseDetailCommand, CollegeCourseDetailDto>
{
    private readonly IUnitOfWork _uow;
    public UpdateCollegeCourseDetailCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<CollegeCourseDetailDto> Handle(UpdateCollegeCourseDetailCommand request, CancellationToken ct)
    {
        var detail = await _uow.CollegeCourseDetails.GetByProjectIdAsync(request.ProjectId, ct)
            ?? throw new NotFoundException("CollegeCourseDetail", request.ProjectId);

        detail.InstitutionName = request.InstitutionName;
        detail.CourseName = request.CourseName;
        detail.CourseCode = request.CourseCode;
        detail.Semester = request.Semester;
        detail.Year = request.Year;
        detail.Credits = request.Credits;
        detail.Professor = request.Professor;
        detail.Room = request.Room;
        detail.Schedule = request.Schedule;
        detail.CurrentGrade = request.CurrentGrade;
        detail.TargetGrade = request.TargetGrade;

        _uow.CollegeCourseDetails.Update(detail);
        await _uow.SaveChangesAsync(ct);
        return CollegeCourseDetailDto.FromEntity(detail);
    }
}
