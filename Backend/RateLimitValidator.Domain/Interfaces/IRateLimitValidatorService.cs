namespace RateLimitValidator.Domain.Interfaces;

public interface IRateLimitValidatorService
{
    string CanSendMessage(string phoneNumber);
}
