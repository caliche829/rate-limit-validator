using RateLimitValidator.Application.Extensions;
using RateLimitValidator.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.BaseRegister(builder.Configuration, builder.Host).RegisterServices();

var app = builder.Build();

app.BaseUse(builder.Configuration);

app.Run();