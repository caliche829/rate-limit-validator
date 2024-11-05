using Microsoft.EntityFrameworkCore;
using RateLimitValidator.Domain.Models;
using System.Reflection;

namespace RateLimitValidator.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<ValidationRequest> ValidationRequests { get; set; }
    public DbSet<ValidationRequestReport> ValidationRequestReports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
