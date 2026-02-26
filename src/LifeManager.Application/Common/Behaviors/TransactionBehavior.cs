using LifeManager.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LifeManager.Application.Common.Behaviors;

public class TransactionBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

    public TransactionBehavior(
        IUnitOfWork unitOfWork,
        ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // Only wrap commands (ICommand marker), not queries
        if (request is not IBaseCommand)
            return await next();

        var requestName = typeof(TRequest).Name;
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            var response = await next();
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Transaction failed for {RequestName}", requestName);
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}

/// <summary>Marker interface — implement on commands that need a transaction.</summary>
public interface IBaseCommand { }
