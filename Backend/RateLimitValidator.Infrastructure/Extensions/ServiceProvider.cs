using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using RateLimitValidator.Domain.Interfaces;
using RateLimitValidator.Infrastructure.Jobs;
using RateLimitValidator.Infrastructure.Services;

namespace RateLimitValidator.Infrastructure.Extensions;

public static class ServiceProvider
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterDBContext(configuration);
        services.RegisterJobs(configuration);
        services.AddSingleton<IRateLimitValidatorService, RateLimitValidatorService>();
        services.AddSingleton<IRegisterRequestService, RegisterRequestService>();

        return services;
    }

    private static IServiceCollection RegisterDBContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(
            options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    opt => opt.EnableRetryOnFailure()
                    .CommandTimeout((int)TimeSpan.FromSeconds(10).TotalSeconds)
                ),
            ServiceLifetime.Singleton);

        return services;
    }

    private static IServiceCollection RegisterJobs(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz(configure =>
        {
            string jobName = nameof(CleanOldRecordsJob);
            var jobKey = new JobKey(jobName);

            var configKey = $"Quartz:{jobName}";
            var cronSchedule = configuration[configKey];

            // Some minor validation
            if (string.IsNullOrEmpty(cronSchedule))
            {
                throw new Exception($"No Quartz.NET Cron schedule found for job in configuration at {configKey}");
            }

            configure
                .AddJob<CleanOldRecordsJob>(opts => opts.WithIdentity(jobKey))
                .AddTrigger(
                    trigger => 
                        trigger.ForJob(jobKey)
                        .WithIdentity(jobName + "-trigger")
                        .WithCronSchedule(cronSchedule)
                );
        });
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        return services;
    }

    public static void RunMigrations(this IServiceProvider services)
    {
        var context = services.GetRequiredService<ApplicationDbContext>();

        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
    }
}
