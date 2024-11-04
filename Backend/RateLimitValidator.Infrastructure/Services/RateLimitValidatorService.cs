using Microsoft.Extensions.Options;
using RateLimitValidator.Domain.Configuration;
using RateLimitValidator.Domain.Interfaces;
using System.Collections.Concurrent;

namespace RateLimitValidator.Infrastructure.Services;

public class RateLimitValidatorService : IRateLimitValidatorService
{
    private readonly RateLimitConfig _config;
    private readonly IRegisterRequestService _registerRequestService;
    private readonly ConcurrentDictionary<string, int> _numberMessageCount = new();
    private int _accountMessageCount;
    private readonly Timer _resetTimer;
    private readonly object _lock = new();

    public RateLimitValidatorService(
        IOptions<RateLimitConfig> config,
        IRegisterRequestService registerRequestService
    )
    {
        _config = config.Value;
        _resetTimer = new Timer(ResetCounts, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        _registerRequestService = registerRequestService;
    }

    public string CanSendMessage(string phoneNumber)
    {
        lock (_lock)
        {
            DateTime now = DateTime.UtcNow;
            if (_accountMessageCount >= _config.MaxMessagesPerAccount)
            {
                _registerRequestService.AddRequest(phoneNumber, now, false);
                return "Too many request per account per second";
            }

            if (_numberMessageCount.TryGetValue(phoneNumber, out int count) && count >= _config.MaxMessagesPerNumber)
            {
                _registerRequestService.AddRequest(phoneNumber, now, false);
                return $"Too many request per phoneNumber {phoneNumber} per second";
            }

            RecordMessage(phoneNumber);
            _registerRequestService.AddRequest(phoneNumber, now, true);

            return string.Empty;
        }
    }

    private void RecordMessage(string phoneNumber)
    {
        _numberMessageCount.AddOrUpdate(phoneNumber, 1, (key, oldValue) => oldValue + 1);
        _accountMessageCount++;
    }

    private void ResetCounts(object? state)
    {
        lock (_lock)
        {
            _numberMessageCount.Clear();
            _accountMessageCount = 0;
        }
    }
}
