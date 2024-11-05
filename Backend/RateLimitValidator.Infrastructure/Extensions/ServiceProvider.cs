using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RateLimitValidator.Domain.Interfaces;
using RateLimitValidator.Infrastructure.Services;

namespace RateLimitValidator.Infrastructure.Extensions;

public static class ServiceProvider
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterDBContext(configuration);
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

    public static void RunMigrations(this IServiceProvider services)
    {
        var context = services.GetRequiredService<ApplicationDbContext>();

        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
    }
}
