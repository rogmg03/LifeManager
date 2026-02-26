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

        // Cycle 1 — Services
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IGoogleAuthService, GoogleAuthService>();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        // Cycle 7 — Services
        services.AddScoped<IFileStorageService, FileStorageService>();

        return services;
    }
}
