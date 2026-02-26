using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.TimeTracking.Commands;

public record DeleteTimeEntryCommand(Guid Id) : IRequest, IBaseCommand;

public class DeleteTimeEntryCommandHandler : IRequestHandler<DeleteTimeEntryCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public DeleteTimeEntryCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    { _uow = uow; _currentUser = currentUser; }

    public async Task Handle(DeleteTimeEntryCommand request, CancellationToken ct)
    {
        var entry = await _uow.TimeEntries.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("TimeEntry", request.Id);

        if (entry.UserId != _currentUser.UserId)
            throw new UnauthorizedAccessException("You do not have permission to delete this time entry.");

        // Cycle 10: Reverse the earned FreeTimeTransaction if one exists
        if (entry.EarnedTransactionId.HasValue)
        {
            var original = await _uow.FreeTimeTransactions.GetByIdAsync(entry.EarnedTransactionId.Value, ct);
            if (original is not null && original.MinutesDelta > 0)
            {
                var currentBalance = await _uow.FreeTimeTransactions
                    .GetLatestBalanceForUserAsync(entry.UserId, ct);

                var reversal = new FreeTimeTransaction
                {
                    UserId = entry.UserId,
                    Type = TransactionType.Spent,
                    MinutesDelta = -original.MinutesDelta,
                    BalanceAfterMinutes = Math.Max(0, currentBalance - original.MinutesDelta),
                    Notes = $"Reversal: deleted time entry {entry.Id}"
                };
                await _uow.FreeTimeTransactions.AddAsync(reversal, ct);
            }
        }

        _uow.TimeEntries.Delete(entry);
        await _uow.SaveChangesAsync(ct);
    }
}
