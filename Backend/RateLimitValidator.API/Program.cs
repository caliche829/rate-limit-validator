using RateLimitValidator.Application.Extensions;
using RateLimitValidator.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.BaseRegister(builder.Configuration, builder.Host).RegisterServices(builder.Configuration);

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    services.RunMigrations();
}

app.BaseUse(builder.Configuration);

app.Run();
