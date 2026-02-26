namespace LifeManager.Domain.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    // Cycle 1
    IUserRepository Users { get; }

    // Cycle 2
    IClientRepository Clients { get; }

    // Cycle 3
    IProjectRepository Projects { get; }

    // Cycle 4
    IPhaseRepository Phases { get; }

    // Cycle 5
    IProjectTaskRepository Tasks { get; }
    ISubtaskRepository Subtasks { get; }
    IRecurrenceRuleRepository RecurrenceRules { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
