using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Options;
using RateLimitValidator.Domain.Configuration;
using System.Net;
using NBomber.CSharp;
using NBomber.Http.CSharp;
using NBomber.Contracts.Stats;

namespace RateLimitValidator.Test;

public class SmsSenderRateLimitTest : IClassFixture<WebApplicationFactory< Program>>
{
    private readonly WebApplicationFactory<Program> _factory = new WebApplicationFactory<Program>();

    [Fact]
    public async void ShouldAllowSend()
    {
        string phoneNumber = "1234567890";
        var client = _factory.CreateClient();
        HttpStatusCode code;

        var response = await client.GetAsync($"/RateLimitValidator/check?phoneNumber={phoneNumber}");
        code = response.StatusCode;

        Assert.Equal(HttpStatusCode.OK, code);
    }

    [Fact]
    public void ShouldThrowRateLimitErrorByPhoneNumber()
    {
        RateLimitConfig? rateLimitConfig = (_factory.Services.GetService(typeof(IOptions<RateLimitConfig>)) as IOptions<RateLimitConfig>)?.Value;

        ArgumentNullException.ThrowIfNull(rateLimitConfig);

        var httpClient = _factory.CreateClient();

        Random rnd = new Random();
        string phoneNumber = rnd.Next().ToString();

        int extraRate = 1;
        int limitSeconds = 2;
        int expectedFailures = extraRate * limitSeconds;
        int rate = rateLimitConfig.MaxMessagesPerNumber + extraRate;

        var simulation = Simulation.Inject(
            rate: rate,
            interval: TimeSpan.FromSeconds(1),
            during: TimeSpan.FromSeconds(limitSeconds));

        var scenario = Scenario.Create("http_scenario", async context =>
        {
            var request = Http.CreateRequest("GET", $"/RateLimitValidator/check?phoneNumber={phoneNumber}");
            var response = await Http.Send(httpClient, request);
            return response;
        })
        .WithoutWarmUp()
        .WithLoadSimulations(simulation);

        NodeStats nodeStats = NBomberRunner.RegisterScenarios(scenario)
                         .WithReportFileName(nameof(ShouldThrowRateLimitErrorByPhoneNumber))
                         .WithReportFormats(ReportFormat.Html, ReportFormat.Txt)
                     .Run();

        Assert.Equal(nodeStats.AllFailCount, expectedFailures);
    }

    [Fact]
    public void ShouldThrowRateLimitErrorByAccount()
    {

        RateLimitConfig? rateLimitConfig = (_factory.Services.GetService(typeof(IOptions<RateLimitConfig>)) as IOptions<RateLimitConfig>)?.Value;

        ArgumentNullException.ThrowIfNull(rateLimitConfig);

        var httpClient = _factory.CreateClient();

        Random rnd = new Random();

        int extraRate = 1;
        int limitSeconds = 2;
        int expectedFailures = extraRate * limitSeconds;
        int rate = rateLimitConfig.MaxMessagesPerAccount + extraRate;

        var simulation = Simulation.Inject(
            rate: rate,
            interval: TimeSpan.FromSeconds(1),
            during: TimeSpan.FromSeconds(limitSeconds));

        var scenario = Scenario.Create("http_scenario", async context =>
        {
            var request = Http.CreateRequest("GET", $"/RateLimitValidator/check?phoneNumber={rnd.Next()}");
            var response = await Http.Send(httpClient, request);
            return response;
        })
        .WithoutWarmUp()
        .WithLoadSimulations(simulation);

        NodeStats nodeStats = NBomberRunner.RegisterScenarios(scenario)
                         .WithReportFileName(nameof(ShouldThrowRateLimitErrorByAccount))
                         .WithReportFormats(ReportFormat.Html, ReportFormat.Txt)
                     .Run();

        Assert.Equal(nodeStats.AllFailCount, expectedFailures);
    }
}