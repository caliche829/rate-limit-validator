using Microsoft.EntityFrameworkCore;
using RateLimitValidator.Domain.Models;

namespace RateLimitValidator.Infrastructure
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
    {
        public DbSet<ValidationRequest> ValidationRequests { get; set; }
    }
}
