using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace RateLimitValidator.Application.Extensions;

public static class AppUse
{
    public static IApplicationBuilder BaseUse(this IApplicationBuilder app, IConfiguration configuration)
    {
        app
            .UsingSwagger()
            .UsingCors()
            .UsingHttpsRedirection()
            .UsingRouting()
            .UsingAuthorization()
            .UsingExceptionHandler()
            .UsingEndpoints();

        return app;
    }

    public static IApplicationBuilder UsingExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler();

        return app;
    }

    public static IApplicationBuilder UsingHttpsRedirection(this IApplicationBuilder app)
    {
        app.UseHttpsRedirection();

        return app;
    }

    public static IApplicationBuilder UsingAuthorization(this IApplicationBuilder app)
    {
        app.UseAuthorization();

        return app;
    }


    public static IApplicationBuilder UsingRouting(this IApplicationBuilder app)
    {
        app.UseRouting();

        return app;
    }

    public static IApplicationBuilder UsingSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }

    public static IApplicationBuilder UsingEndpoints(this IApplicationBuilder app)
    {
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        return app;
    }

    public static IApplicationBuilder UsingCors(this IApplicationBuilder app)
    {
        app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

        return app;
    }
}
