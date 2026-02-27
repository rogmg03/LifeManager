using LifeManager.Application.Common.Interfaces;
using LifeManager.Domain.Interfaces.Repositories;
using LifeManager.Infrastructure.Persistence;
using LifeManager.Infrastructure.Persistence.Interceptors;
using LifeManager.Infrastructure.Persistence.Repositories;
using LifeManager.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LifeManager.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Interceptors
        services.AddScoped<AuditableEntityInterceptor>();
        services.AddScoped<DomainEventInterceptor>();

        // EF Core
        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            options.AddInterceptors(
                sp.GetRequiredService<AuditableEntityInterceptor>(),
                sp.GetRequiredService<DomainEventInterceptor>());
        });

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Cycle 1
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserSettingsRepository, UserSettingsRepository>();

        // Cycle 2
        services.AddScoped<IClientRepository, ClientRepository>();

        // Cycle 3
        services.AddScoped<IProjectRepository, ProjectRepository>();

        // Cycle 4
        services.AddScoped<IPhaseRepository, PhaseRepository>();

        // Cycle 5
        services.AddScoped<IProjectTaskRepository, ProjectTaskRepository>();
        services.AddScoped<ISubtaskRepository, SubtaskRepository>();
        services.AddScoped<IRecurrenceRuleRepository, RecurrenceRuleRepository>();

        // Cycle 6
        services.AddScoped<ILabelRepository, LabelRepository>();

        // Cycle 7
        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<IAttachmentRepository, AttachmentRepository>();

        // Cycle 8
        services.AddScoped<IActivityEntryRepository, ActivityEntryRepository>();

        // Cycle 9
        services.AddScoped<ITimeEntryRepository, TimeEntryRepository>();

        // Cycle 10
        services.AddScoped<IFreeTimeTransactionRepository, FreeTimeTransactionRepository>();
        services.AddScoped<IFreeTimeRatioRepository, FreeTimeRatioRepository>();

        // Cycle 11
        services.AddScoped<IScheduleBlockRepository, ScheduleBlockRepository>();

        // Cycle 12
        services.AddScoped<IReminderRepository, ReminderRepository>();
        services.AddScoped<INotificationLogRepository, NotificationLogRepository>();

        // Cycle 13
        services.AddScoped<ICollegeCourseDetailRepository, CollegeCourseDetailRepository>();

        // Cycle 14
        services.AddScoped<IOnlineCourseDetailRepository, OnlineCourseDetailRepository>();

        // Cycle 15
        services.AddScoped<IWorkInitiativeDetailRepository, WorkInitiativeDetailRepository>();

        // Cycle 16
        services.AddScoped<IRoutineRepository, RoutineRepository>();
        services.AddScoped<IWorkoutLogRepository, WorkoutLogRepository>();

        // Cycle 17
        services.AddScoped<IExerciseGoalRepository, ExerciseGoalRepository>();
        services.AddScoped<IProgressEntryRepository, ProgressEntryRepository>();

        // Cycle 1 — Services
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IGoogleAuthService, GoogleAuthService>();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        // Cycle 7 — Services
        services.AddScoped<IFileStorageService, FileStorageService>();

        // Cycle 10 — Services
        services.AddScoped<IFreeTimeCalculator, FreeTimeCalculator>();

        // Cycle 18 — Dapper read infrastructure
        services.AddSingleton<IDbConnectionFactory, NpgsqlConnectionFactory>();
        services.AddScoped<IDashboardReadService, DashboardReadService>();

        // Cycle 19 — Analytics read service
        services.AddScoped<IAnalyticsReadService, AnalyticsReadService>();

        return services;
    }
}
