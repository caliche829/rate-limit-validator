using RateLimitValidator.Domain.Interfaces;
using RateLimitValidator.Domain.Models;

namespace RateLimitValidator.Infrastructure.Services;

public class RegisterRequestService(ApplicationDbContext dbContext) : IRegisterRequestService
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public void AddRequest(string phoneNumber, DateTime time, bool isSuccess)
    {
        _dbContext.ValidationRequests.Add(new ValidationRequest()
        {
            PhoneNumber = phoneNumber,
            Time = time,
            IsSuccess = isSuccess
        });

        _dbContext.SaveChanges();
    }
}
