using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.FreeTime.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Events;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.FreeTime.Commands;

public record CashOutFreeTimeCommand(int Minutes, string? Notes) : IRequest<FreeTimeTransactionDto>, IBaseCommand;

public class CashOutFreeTimeCommandHandler : IRequestHandler<CashOutFreeTimeCommand, FreeTimeTransactionDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public CashOutFreeTimeCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    { _uow = uow; _currentUser = currentUser; }

    public async Task<FreeTimeTransactionDto> Handle(CashOutFreeTimeCommand request, CancellationToken ct)
    {
        if (request.Minutes <= 0)
            throw new ConflictException("Minutes to cash out must be greater than zero.");

        var userId = _currentUser.UserId;
        var currentBalance = await _uow.FreeTimeTransactions.GetLatestBalanceForUserAsync(userId, ct);

        if (currentBalance < request.Minutes)
            throw new ConflictException(
                $"Insufficient free time balance. Available: {currentBalance} min, requested: {request.Minutes} min.");

        var transaction = new FreeTimeTransaction
        {
            UserId = userId,
            Type = TransactionType.Spent,
            MinutesDelta = -request.Minutes,
            BalanceAfterMinutes = currentBalance - request.Minutes,
            Notes = request.Notes ?? $"Cashed out {request.Minutes} min of free time"
        };

        transaction.AddDomainEvent(new FreeTimeCashedOutEvent(
            transaction.Id, userId, request.Minutes, transaction.BalanceAfterMinutes));

        await _uow.FreeTimeTransactions.AddAsync(transaction, ct);
        await _uow.SaveChangesAsync(ct);

        return FreeTimeTransactionDto.FromEntity(transaction);
    }
}
