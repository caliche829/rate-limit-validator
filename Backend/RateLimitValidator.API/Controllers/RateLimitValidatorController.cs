using Microsoft.AspNetCore.Mvc;
using RateLimitValidator.Domain.Interfaces;

namespace RateLimitValidator.API.Controllers;

[ApiController]
[Route("[controller]")]
public class RateLimitValidatorController(
    ILogger<RateLimitValidatorController> logger,
    IRateLimitValidatorService rateLimiterService
) : ControllerBase
{

    private readonly ILogger<RateLimitValidatorController> _logger = logger;
    private readonly IRateLimitValidatorService _rateLimiterService = rateLimiterService;

    [HttpGet("check")]
    public IActionResult CheckMessageSend(string phoneNumber)
    {
        string errorMessage = _rateLimiterService.CanSendMessage(phoneNumber);
        
        if (!string.IsNullOrEmpty(errorMessage))
        {
            return Problem
            (
                detail: errorMessage,
                statusCode: (int?)System.Net.HttpStatusCode.TooManyRequests,
                title: "Too many request"
            );
        }

        return Ok();
    }
}
