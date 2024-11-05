using RateLimitValidator.Domain.Dto;
using RateLimitValidator.Domain.Models;

namespace RateLimitValidator.Domain.Interfaces;

public interface IRequestReportService
{
    Task<ReportResult> GetReport(RequestReportQuery filters);
}
