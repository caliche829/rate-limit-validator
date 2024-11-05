namespace RateLimitValidator.Domain.Interfaces;

public interface IRegisterRequestService
{
    void AddRequest(string phoneNumber, DateTime time, bool isSuccess, string? errorMessage);
}
