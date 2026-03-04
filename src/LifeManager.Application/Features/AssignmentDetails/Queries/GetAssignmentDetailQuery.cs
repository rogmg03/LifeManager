using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.AssignmentDetails.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.AssignmentDetails.Queries;

public record GetAssignmentDetailQuery(Guid TaskId) : IRequest<AssignmentDetailDto>;

public class GetAssignmentDetailQueryHandler : IRequestHandler<GetAssignmentDetailQuery, AssignmentDetailDto>
{
    private readonly IUnitOfWork _uow;
    public GetAssignmentDetailQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<AssignmentDetailDto> Handle(GetAssignmentDetailQuery request, CancellationToken ct)
    {
        var detail = await _uow.AssignmentDetails.GetByTaskIdAsync(request.TaskId, ct)
            ?? throw new NotFoundException("AssignmentDetail", request.TaskId);
        return AssignmentDetailDto.FromEntity(detail);
    }
}
