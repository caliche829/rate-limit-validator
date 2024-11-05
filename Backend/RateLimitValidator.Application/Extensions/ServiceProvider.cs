using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using RateLimitValidator.Application.Exceptions;
using RateLimitValidator.Domain.Configuration;
using StackExchange.Redis;

namespace RateLimitValidator.Application.Extensions;

public static class ServiceProvider
{
    public static IServiceCollection BaseRegister(this IServiceCollection services,
        IConfiguration configuration, IHostBuilder hostBuilder)
    {
        services
            .RegisterControllers()
            .RegisterLog(configuration, hostBuilder)
            .RegisterRedis(configuration)
            .RegisterRateLimitConfig(configuration)
            .RegisterGlobalErrorHandler()
            .RegisterSwagger();

        return services;
    }

    private static IServiceCollection RegisterControllers(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddEndpointsApiExplorer();

        return services;
    }

    public static IServiceCollection RegisterLog(this IServiceCollection services,
        IConfiguration configuration, IHostBuilder hostBuilder)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
            .WriteTo.File("../Logs/log.txt", restrictedToMinimumLevel: LogEventLevel.Information, rollingInterval: RollingInterval.Day)
            .WriteTo.File("../Logs/logError.txt", restrictedToMinimumLevel: LogEventLevel.Error, rollingInterval: RollingInterval.Day)
            .CreateLogger();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddSerilog(dispose: true);
        });

        hostBuilder.UseSerilog();

        return services;
    }

    private static IServiceCollection RegisterGlobalErrorHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();

        services.AddProblemDetails();

        return services;
    }

    public static IServiceCollection RegisterRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(provider =>
        {
            var cfg = configuration.GetConnectionString("RedisConnection");
            return ConnectionMultiplexer.Connect(cfg!);
        });

        return services;
    }

    private static IServiceCollection RegisterSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen();

        return services;
    }

    private static IServiceCollection RegisterRateLimitConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RateLimitConfig>(configuration.GetSection("RateLimitDefinition"));

        return services;
    }
}
