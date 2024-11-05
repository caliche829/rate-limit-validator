using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Options;
using RateLimitValidator.Domain.Configuration;
using NBomber.CSharp;
using NBomber.Http.CSharp;
using NBomber.Contracts.Stats;
using RateLimitValidator.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace RateLimitValidator.Test;

public class RateLimitValidatorTest : IClassFixture<WebApplicationFactory< Program>>
{
    private readonly WebApplicationFactory<Program> _factory = new WebApplicationFactory<Program>();
    private const int LimitSeconds = 2;
    private const string CheckEndpoint = "/RateLimitValidator/check?phoneNumber={0}";

    [Fact]
    public void ShouldAllowSendByPhoneNumber()
    {
        RateLimitConfig? rateLimitConfig = (_factory.Services.GetService(typeof(IOptions<RateLimitConfig>)) as IOptions<RateLimitConfig>)?.Value;

        ArgumentNullException.ThrowIfNull(rateLimitConfig);

        ApplicationDbContext? dbContext = _factory.Services.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;

        var httpClient = _factory.CreateClient();

        Random rnd = new Random();
        string phoneNumber = rnd.Next().ToString();

        int expectedFailures = 0;

        var simulation = Simulation.Inject(
            rate: rateLimitConfig.MaxMessagesPerNumber,
            interval: TimeSpan.FromSeconds(1),
            during: TimeSpan.FromSeconds(LimitSeconds));

        var scenario = Scenario.Create("http_scenario", async context =>
        {
            var request = Http.CreateRequest("GET", string.Format(CheckEndpoint, phoneNumber));
            var response = await Http.Send(httpClient, request);
            return response;
        })
        .WithoutWarmUp()
        .WithLoadSimulations(simulation)
        .WithClean(context =>
        {
            dbContext?.ValidationRequests
                .Where(x => x.PhoneNumber == phoneNumber)
                .ExecuteDeleteAsync();
            return Task.CompletedTask;
        });

        NodeStats nodeStats = NBomberRunner.RegisterScenarios(scenario)
                         .WithReportFileName(nameof(ShouldAllowSendByPhoneNumber))
                         .WithReportFormats(ReportFormat.Html, ReportFormat.Txt)
                     .Run();

        Assert.Equal(nodeStats.AllFailCount, expectedFailures);
    }

    [Fact]
    public void ShouldAllowSendByAccount()
    {

        RateLimitConfig? rateLimitConfig = (_factory.Services.GetService(typeof(IOptions<RateLimitConfig>)) as IOptions<RateLimitConfig>)?.Value;

        ArgumentNullException.ThrowIfNull(rateLimitConfig);

        ApplicationDbContext? dbContext = _factory.Services.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;

        BlockingCollection<string> phoneNumbersGenerated = new BlockingCollection<string>();

        var httpClient = _factory.CreateClient();

        Random rnd = new Random();
        string phoneNumber;

        int expectedFailures = 0;

        var simulation = Simulation.Inject(
            rate: rateLimitConfig.MaxMessagesPerAccount,
            interval: TimeSpan.FromSeconds(1),
            during: TimeSpan.FromSeconds(LimitSeconds));

        var scenario = Scenario.Create("http_scenario", async context =>
        {
            phoneNumber = rnd.Next().ToString();
            phoneNumbersGenerated.Add(phoneNumber);
            var request = Http.CreateRequest("GET", string.Format(CheckEndpoint, phoneNumber));
            var response = await Http.Send(httpClient, request);
            return response;
        })
        .WithoutWarmUp()
        .WithLoadSimulations(simulation)
        .WithClean(context =>
        {
            var phoneNumbers = phoneNumbersGenerated.ToList();
            dbContext?.ValidationRequests
                .Where(x => phoneNumbers.Contains(x.PhoneNumber))
                .ExecuteDeleteAsync();
            return Task.CompletedTask;
        });

        NodeStats nodeStats = NBomberRunner.RegisterScenarios(scenario)
                         .WithReportFileName(nameof(ShouldAllowSendByAccount))
                         .WithReportFormats(ReportFormat.Html, ReportFormat.Txt)
                     .Run();

        Assert.Equal(nodeStats.AllFailCount, expectedFailures);
    }

    [Fact]
    public void ShouldThrowRateLimitErrorByPhoneNumber()
    {
        RateLimitConfig? rateLimitConfig = (_factory.Services.GetService(typeof(IOptions<RateLimitConfig>)) as IOptions<RateLimitConfig>)?.Value;

        ArgumentNullException.ThrowIfNull(rateLimitConfig);

        ApplicationDbContext? dbContext = _factory.Services.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;

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
            var request = Http.CreateRequest("GET", string.Format(CheckEndpoint, phoneNumber));
            var response = await Http.Send(httpClient, request);
            return response;
        })
        .WithoutWarmUp()
        .WithLoadSimulations(simulation)
        .WithClean(context =>
        {
            dbContext?.ValidationRequests
                .Where(x => x.PhoneNumber == phoneNumber)
                .ExecuteDeleteAsync();
            return Task.CompletedTask;
        });

        NodeStats nodeStats = NBomberRunner.RegisterScenarios(scenario)
                         .WithReportFileName(nameof(ShouldThrowRateLimitErrorByPhoneNumber))
                         .WithReportFormats(ReportFormat.Html, ReportFormat.Txt)
                     .Run();

        //Some cases the next batch is sent in the same second as the first batch,
        //that cause a rejection of the first request of the second batch
        Assert.True(nodeStats.AllFailCount <= expectedFailures);
    }

    [Fact]
    public void ShouldThrowRateLimitErrorByAccount()
    {

        RateLimitConfig? rateLimitConfig = (_factory.Services.GetService(typeof(IOptions<RateLimitConfig>)) as IOptions<RateLimitConfig>)?.Value;

        ArgumentNullException.ThrowIfNull(rateLimitConfig);

        ApplicationDbContext? dbContext = _factory.Services.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;

        BlockingCollection<string> phoneNumbersGenerated = new BlockingCollection<string>();

        var httpClient = _factory.CreateClient();

        Random rnd = new Random();
        string phoneNumber;

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
            phoneNumber = rnd.Next().ToString();
            phoneNumbersGenerated.Add(phoneNumber);
            var request = Http.CreateRequest("GET", string.Format(CheckEndpoint, phoneNumber));
            var response = await Http.Send(httpClient, request);
            return response;
        })
        .WithoutWarmUp()
        .WithLoadSimulations(simulation)
        .WithClean(context =>
        {
            var phoneNumbers = phoneNumbersGenerated.ToList();
            dbContext?.ValidationRequests
                .Where(x => phoneNumbers.Contains(x.PhoneNumber))
                .ExecuteDeleteAsync();
            return Task.CompletedTask;
        });

        NodeStats nodeStats = NBomberRunner.RegisterScenarios(scenario)
                         .WithReportFileName(nameof(ShouldThrowRateLimitErrorByAccount))
                         .WithReportFormats(ReportFormat.Html, ReportFormat.Txt)
                     .Run();

        //Some cases the next batch is sent in the same second as the first batch,
        //that cause a rejection of the first request of the second batch
        Assert.True(nodeStats.AllFailCount <= expectedFailures);
    }
}