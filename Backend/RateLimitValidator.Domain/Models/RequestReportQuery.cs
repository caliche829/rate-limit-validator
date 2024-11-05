namespace RateLimitValidator.Domain.Models;

public class RequestReportQuery
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? PhoneNumber { get; set; } 
    public string? Date { get; set; } 
    public string? Time { get; set; }
}
