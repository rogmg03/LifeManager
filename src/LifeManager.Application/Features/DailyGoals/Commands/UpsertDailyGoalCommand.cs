using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.DailyGoals.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.DailyGoals.Commands;

public record UpsertDailyGoalCommand(DailyGoalCategory Category, int GoalMinutes) : IRequest<DailyGoalDto>, IBaseCommand;

public class UpsertDailyGoalCommandHandler : IRequestHandler<UpsertDailyGoalCommand, DailyGoalDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public UpsertDailyGoalCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<DailyGoalDto> Handle(UpsertDailyGoalCommand request, CancellationToken ct)
    {
        var existing = await _uow.DailyGoals.GetByUserAndCategoryAsync(_currentUser.UserId, request.Category, ct);

        if (existing is null)
        {
            var goal = new DailyGoal
            {
                UserId = _currentUser.UserId,
                Category = request.Category,
                GoalMinutes = request.GoalMinutes
            };
            await _uow.DailyGoals.AddAsync(goal, ct);
            await _uow.SaveChangesAsync(ct);
            return DailyGoalDto.FromEntity(goal);
        }

        existing.GoalMinutes = request.GoalMinutes;
        _uow.DailyGoals.Update(existing);
        await _uow.SaveChangesAsync(ct);
        return DailyGoalDto.FromEntity(existing);
    }
}
