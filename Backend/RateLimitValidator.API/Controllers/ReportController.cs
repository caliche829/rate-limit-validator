using Microsoft.AspNetCore.Mvc;
using RateLimitValidator.Domain.Interfaces;
using RateLimitValidator.Domain.Models;

namespace RateLimitValidator.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController(
    ILogger<ReportController> logger,
    IRequestReportService requestReportService
) : ControllerBase
{
    private readonly ILogger<ReportController> _logger = logger;
    private readonly IRequestReportService _requestReportService = requestReportService;

    [HttpPost]
    public async Task<IActionResult> GetReportRequests([FromBody] RequestReportQuery requestReportQuery)
    {
        var result = await _requestReportService.GetReport(requestReportQuery);
        return Ok(result);
    }
}
