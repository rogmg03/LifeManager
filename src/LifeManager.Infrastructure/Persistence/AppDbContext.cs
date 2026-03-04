using LifeManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Cycle 1
    public DbSet<User> Users => Set<User>();
    public DbSet<UserSettings> UserSettings => Set<UserSettings>();

    // Cycle 2
    public DbSet<Client> Clients => Set<Client>();

    // Cycle 3
    public DbSet<Project> Projects => Set<Project>();

    // Cycle 4
    public DbSet<Phase> Phases => Set<Phase>();

    // Cycle 5
    public DbSet<ProjectTask> Tasks => Set<ProjectTask>();
    public DbSet<Subtask> Subtasks => Set<Subtask>();
    public DbSet<RecurrenceRule> RecurrenceRules => Set<RecurrenceRule>();

    // Cycle 6
    public DbSet<Label> Labels => Set<Label>();
    public DbSet<ProjectLabel> ProjectLabels => Set<ProjectLabel>();
    public DbSet<TaskLabel> TaskLabels => Set<TaskLabel>();

    // Cycle 7
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<Attachment> Attachments => Set<Attachment>();

    // Cycle 8
    public DbSet<ActivityEntry> ActivityEntries => Set<ActivityEntry>();

    // Cycle 9
    public DbSet<TimeEntry> TimeEntries => Set<TimeEntry>();

    // Cycle 10
    public DbSet<FreeTimeTransaction> FreeTimeTransactions => Set<FreeTimeTransaction>();
    public DbSet<FreeTimeRatio> FreeTimeRatios => Set<FreeTimeRatio>();

    // Cycle 11
    public DbSet<ScheduleBlock> ScheduleBlocks => Set<ScheduleBlock>();

    // Cycle 12
    public DbSet<Reminder> Reminders => Set<Reminder>();
    public DbSet<NotificationLog> NotificationLogs => Set<NotificationLog>();

    // Cycle 13
    public DbSet<CollegeCourseDetail> CollegeCourseDetails => Set<CollegeCourseDetail>();

    // Cycle 14
    public DbSet<OnlineCourseDetail> OnlineCourseDetails => Set<OnlineCourseDetail>();

    // Cycle 15
    public DbSet<WorkInitiativeDetail> WorkInitiativeDetails => Set<WorkInitiativeDetail>();

    // Exercise Redesign (E1)
    public DbSet<Routine> Routines => Set<Routine>();
    public DbSet<RoutineItem> RoutineItems => Set<RoutineItem>();
    public DbSet<WorkoutSession> WorkoutSessions => Set<WorkoutSession>();
    public DbSet<WorkoutSet> WorkoutSets => Set<WorkoutSet>();

    // Cycle 17
    public DbSet<ExerciseGoal> ExerciseGoals => Set<ExerciseGoal>();
    public DbSet<ProgressEntry> ProgressEntries => Set<ProgressEntry>();

    // Session 2A
    public DbSet<AssignmentDetail> AssignmentDetails => Set<AssignmentDetail>();
    public DbSet<CourseModule> CourseModules => Set<CourseModule>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
