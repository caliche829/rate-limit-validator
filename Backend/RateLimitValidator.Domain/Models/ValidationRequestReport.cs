namespace RateLimitValidator.Domain.Models;

public class ValidationRequestReport
{
    public required string PhoneNumber { get; set; }
    public DateTime Time { get; set; }
    public int TotalSuccess { get; set; }
    public int TotalError { get; set; }
}
