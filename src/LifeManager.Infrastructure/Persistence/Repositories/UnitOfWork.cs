using LifeManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace LifeManager.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(
        AppDbContext context,
        IUserRepository users,
        IClientRepository clients,
        IProjectRepository projects,
        IPhaseRepository phases,
        IProjectTaskRepository tasks,
        ISubtaskRepository subtasks,
        IRecurrenceRuleRepository recurrenceRules,
        ILabelRepository labels,
        IDocumentRepository documents,
        IAttachmentRepository attachments,
        IActivityEntryRepository activityEntries,
        ITimeEntryRepository timeEntries,
        IFreeTimeTransactionRepository freeTimeTransactions,
        IFreeTimeRatioRepository freeTimeRatios,
        IScheduleBlockRepository scheduleBlocks,
        IReminderRepository reminders,
        INotificationLogRepository notificationLogs,
        ICollegeCourseDetailRepository collegeCourseDetails,
        IOnlineCourseDetailRepository onlineCourseDetails)
    {
        _context = context;
        Users = users;
        Clients = clients;
        Projects = projects;
        Phases = phases;
        Tasks = tasks;
        Subtasks = subtasks;
        RecurrenceRules = recurrenceRules;
        Labels = labels;
        Documents = documents;
        Attachments = attachments;
        ActivityEntries = activityEntries;
        TimeEntries = timeEntries;
        FreeTimeTransactions = freeTimeTransactions;
        FreeTimeRatios = freeTimeRatios;
        ScheduleBlocks = scheduleBlocks;
        Reminders = reminders;
        NotificationLogs = notificationLogs;
        CollegeCourseDetails = collegeCourseDetails;
        OnlineCourseDetails = onlineCourseDetails;
    }

    // Cycle 1
    public IUserRepository Users { get; }

    // Cycle 2
    public IClientRepository Clients { get; }

    // Cycle 3
    public IProjectRepository Projects { get; }

    // Cycle 4
    public IPhaseRepository Phases { get; }

    // Cycle 5
    public IProjectTaskRepository Tasks { get; }
    public ISubtaskRepository Subtasks { get; }
    public IRecurrenceRuleRepository RecurrenceRules { get; }

    // Cycle 6
    public ILabelRepository Labels { get; }

    // Cycle 7
    public IDocumentRepository Documents { get; }
    public IAttachmentRepository Attachments { get; }

    // Cycle 8
    public IActivityEntryRepository ActivityEntries { get; }

    // Cycle 9
    public ITimeEntryRepository TimeEntries { get; }

    // Cycle 10
    public IFreeTimeTransactionRepository FreeTimeTransactions { get; }
    public IFreeTimeRatioRepository FreeTimeRatios { get; }

    // Cycle 11
    public IScheduleBlockRepository ScheduleBlocks { get; }

    // Cycle 12
    public IReminderRepository Reminders { get; }
    public INotificationLogRepository NotificationLogs { get; }

    // Cycle 13
    public ICollegeCourseDetailRepository CollegeCourseDetails { get; }

    // Cycle 14
    public IOnlineCourseDetailRepository OnlineCourseDetails { get; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        => _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
