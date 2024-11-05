using RateLimitValidator.Domain.Interfaces;
using RateLimitValidator.Domain.Models;

namespace RateLimitValidator.Infrastructure.Services;

public class RegisterRequestService(ApplicationDbContext dbContext) : IRegisterRequestService
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public void AddRequest(string phoneNumber, DateTime time, bool isSuccess, string? errorMessage)
    {
        _dbContext.ValidationRequests.Add(new ValidationRequest()
        {
            Id = Guid.NewGuid(),
            PhoneNumber = phoneNumber,
            Time = time,
            IsSuccess = isSuccess,
            ErrorMessage = errorMessage
        });

        _dbContext.SaveChanges();
    }
}
