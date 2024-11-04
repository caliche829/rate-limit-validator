namespace RateLimitValidator.Domain.Models;

public class ValidationRequest
{
    public required string PhoneNumber { get; set; }
    public DateTime Time { get; set; }
    public bool IsSuccess { get; set; }
}
