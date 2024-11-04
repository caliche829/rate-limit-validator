namespace RateLimitValidator.Domain.Configuration;

public class RateLimitConfig
{
    /// <summary>
    /// Max messages per phone number per second
    /// </summary>
    public int MaxMessagesPerNumber { get; set; }

    /// <summary>
    /// Max messages for the entire account per second
    /// </summary>
    public int MaxMessagesPerAccount { get; set; }
}
