using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RateLimitValidator.Domain.Models;

namespace RateLimitValidator.Infrastructure.DbModelConfigurations;

public class ValidationRequestConfiguration : IEntityTypeConfiguration<ValidationRequest>
{
    public void Configure(EntityTypeBuilder<ValidationRequest> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasIndex(b => new { b.PhoneNumber, b.Time })
            .HasDatabaseName("IX_ValidationRequest_PhoneNumber_Time_Ascending");
    }
}
