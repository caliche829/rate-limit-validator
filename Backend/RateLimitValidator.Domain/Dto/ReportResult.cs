using RateLimitValidator.Domain.Models;

namespace RateLimitValidator.Domain.Dto;

public record ReportResult (List<ValidationRequestReport> Data, int TotalRecords);
