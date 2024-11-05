using Microsoft.AspNetCore.Mvc;
using RateLimitValidator.Domain.Interfaces;

namespace RateLimitValidator.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ReportController(
    ILogger<ReportController> logger,
    IRateLimitValidatorService rateLimiterService
) : ControllerBase
{

    private readonly ILogger<ReportController> _logger = logger;
    private readonly IRateLimitValidatorService _rateLimiterService = rateLimiterService;

    [HttpPost("")]
    public IActionResult CheckMessageSend(string phoneNumber)
    {
        string errorMessage = _rateLimiterService.CanSendMessage(phoneNumber);
        
        if (!string.IsNullOrEmpty(errorMessage))
        {
            return Problem
            (
                detail: errorMessage,
                statusCode: (int?)System.Net.HttpStatusCode.TooManyRequests,
                title: errorMessage
            );
        }

        return Ok();
    }
}
