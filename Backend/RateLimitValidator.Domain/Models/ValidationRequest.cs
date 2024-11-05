namespace RateLimitValidator.Domain.Models;

public class ValidationRequest
{
    public Guid Id { get; set; }
    public required string PhoneNumber { get; set; }
    public DateTime Time { get; set; }
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
}
