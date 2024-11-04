using Microsoft.Extensions.DependencyInjection;
using RateLimitValidator.Domain.Interfaces;
using RateLimitValidator.Infrastructure.Services;

namespace RateLimitValidator.Infrastructure.Extensions;

public static class ServiceProvider
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IRateLimitValidatorService, RateLimitValidatorService>();

        return services;
    }
}
