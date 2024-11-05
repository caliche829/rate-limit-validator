using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RateLimitValidator.Domain.Models;

namespace RateLimitValidator.Infrastructure.DbModelConfigurations;

public class ValidationRequestReportConfiguration : IEntityTypeConfiguration<ValidationRequestReport>
{
    public void Configure(EntityTypeBuilder<ValidationRequestReport> builder)
    {
        builder.HasNoKey();
    }
}
