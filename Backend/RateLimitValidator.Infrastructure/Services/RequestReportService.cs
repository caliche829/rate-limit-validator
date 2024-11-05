using Microsoft.EntityFrameworkCore;
using RateLimitValidator.Domain.Dto;
using RateLimitValidator.Domain.Interfaces;
using RateLimitValidator.Domain.Models;

namespace RateLimitValidator.Infrastructure.Services;

public class RequestReportService(ApplicationDbContext dbContext) : IRequestReportService
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<ReportResult> GetReport(RequestReportQuery filters)
    {
        var query = _dbContext.ValidationRequestReports.AsQueryable();

        if (!string.IsNullOrEmpty(filters.PhoneNumber))
        {
            query = query.Where(p => p.PhoneNumber.Contains(filters.PhoneNumber));
        }

        if (!string.IsNullOrEmpty(filters.Date))
        {
            if (!string.IsNullOrEmpty(filters.Time))
                filters.Date = $"{filters.Date} {filters.Time}";
            
            if (DateTime.TryParse(filters.Date, out DateTime dateFilter))
                query = query.Where(p => p.Time >= dateFilter);
        }

        var totalRecords = await query.CountAsync();
        var records = await query.Skip((filters.Page - 1) * filters.PageSize).Take(filters.PageSize).ToListAsync();

        return new ReportResult(records, totalRecords);
    }
}
