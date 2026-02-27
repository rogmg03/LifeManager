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

    // Cycle 6
    ILabelRepository Labels { get; }

    // Cycle 7
    IDocumentRepository Documents { get; }
    IAttachmentRepository Attachments { get; }

    // Cycle 8
    IActivityEntryRepository ActivityEntries { get; }

    // Cycle 9
    ITimeEntryRepository TimeEntries { get; }

    // Cycle 10
    IFreeTimeTransactionRepository FreeTimeTransactions { get; }
    IFreeTimeRatioRepository FreeTimeRatios { get; }

    // Cycle 11
    IScheduleBlockRepository ScheduleBlocks { get; }

    // Cycle 12
    IReminderRepository Reminders { get; }
    INotificationLogRepository NotificationLogs { get; }

    // Cycle 13
    ICollegeCourseDetailRepository CollegeCourseDetails { get; }

    // Cycle 14
    IOnlineCourseDetailRepository OnlineCourseDetails { get; }

    // Cycle 15
    IWorkInitiativeDetailRepository WorkInitiativeDetails { get; }

    // Cycle 16
    IRoutineRepository Routines { get; }
    IWorkoutLogRepository WorkoutLogs { get; }

    // Cycle 17
    IExerciseGoalRepository ExerciseGoals { get; }
    IProgressEntryRepository ProgressEntries { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
