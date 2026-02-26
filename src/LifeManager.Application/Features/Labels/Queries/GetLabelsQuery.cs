using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Labels.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Labels.Queries;

public record GetLabelsQuery : IRequest<List<LabelDto>>;

public class GetLabelsQueryHandler : IRequestHandler<GetLabelsQuery, List<LabelDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GetLabelsQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    { _uow = uow; _currentUser = currentUser; }

    public async Task<List<LabelDto>> Handle(GetLabelsQuery request, CancellationToken ct)
    {
        var labels = await _uow.Labels.GetAllByUserIdAsync(_currentUser.UserId, ct);
        return labels.Select(LabelDto.FromEntity).ToList();
    }
}
