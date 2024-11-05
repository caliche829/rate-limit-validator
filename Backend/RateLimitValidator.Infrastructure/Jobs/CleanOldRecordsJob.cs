using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;

namespace RateLimitValidator.Infrastructure.Jobs;

[DisallowConcurrentExecution]
public class CleanOldRecordsJob 
(
    ILogger<CleanOldRecordsJob> logger, 
    ApplicationDbContext dbContext, 
    IConfiguration configuration
) : IJob
{
    private readonly IConfiguration _configuration = configuration;
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly ILogger<CleanOldRecordsJob> _logger = logger;

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Cleaning old records");

        int.TryParse(_configuration["DaysToClean"], out int daysToClean);

        DateTime limit = DateTime.Parse(DateTime.UtcNow.ToString("yyyy-MM-dd")).AddDays(-daysToClean);

        _dbContext.ValidationRequests
            .Where(x => x.Time <= limit)
            .ExecuteDeleteAsync();

        return Task.CompletedTask;
    }
}
