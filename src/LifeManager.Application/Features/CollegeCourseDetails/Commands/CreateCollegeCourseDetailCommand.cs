using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.CollegeCourseDetails.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.CollegeCourseDetails.Commands;

public record CreateCollegeCourseDetailCommand(
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

public class CreateCollegeCourseDetailCommandHandler
    : IRequestHandler<CreateCollegeCourseDetailCommand, CollegeCourseDetailDto>
{
    private readonly IUnitOfWork _uow;
    public CreateCollegeCourseDetailCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<CollegeCourseDetailDto> Handle(CreateCollegeCourseDetailCommand request, CancellationToken ct)
    {
        var project = await _uow.Projects.GetByIdAsync(request.ProjectId, ct)
            ?? throw new NotFoundException("Project", request.ProjectId);

        if (project.Type != ProjectType.CollegeCourse)
            throw new BadRequestException("Project is not a College Course type.");

        var existing = await _uow.CollegeCourseDetails.GetByProjectIdAsync(request.ProjectId, ct);
        if (existing is not null)
            throw new ConflictException("College course detail already exists for this project.");

        var detail = new CollegeCourseDetail
        {
            ProjectId = request.ProjectId,
            InstitutionName = request.InstitutionName,
            CourseName = request.CourseName,
            CourseCode = request.CourseCode,
            Semester = request.Semester,
            Year = request.Year,
            Credits = request.Credits,
            Professor = request.Professor,
            Room = request.Room,
            Schedule = request.Schedule,
            CurrentGrade = request.CurrentGrade,
            TargetGrade = request.TargetGrade
        };

        await _uow.CollegeCourseDetails.AddAsync(detail, ct);
        await _uow.SaveChangesAsync(ct);
        return CollegeCourseDetailDto.FromEntity(detail);
    }
}
