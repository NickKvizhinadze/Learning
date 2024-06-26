using System.Globalization;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RiverBooks.Reporting.Domain;
using RiverBooks.Reporting.Interfaces;

namespace RiverBooks.Reporting.Infrastructure;

internal class DefaultSalesReportingService(
    ILogger<DefaultSalesReportingService> logger,
    IConfiguration configuration)
    : ISalesReportingService
{
    private readonly string? _connectionString = configuration.GetConnectionString("ReportingConnectionString");

    public async Task<TopBooksByMonthReport> GetTopBooksByMonthReportAsync(int year, int month)
    {
        string sql = @"select BookId, Title, Author, UnitsSold as Units, TotalSales as Sales
                    from Reporting.MonthlyBookSales
                    where Month = @month and Year = @year
                    ORDER BY TotalSales DESC
                    ";
        await using var conn = new SqlConnection(_connectionString);
        logger.LogInformation("Executing query: {sql}", sql);
        var results = (await conn.QueryAsync<BookSalesResult>(sql, new { month, year }))
            .ToList();

        var report = new TopBooksByMonthReport
        {
            Year = year,
            Month = month,
            MonthName = CultureInfo.GetCultureInfo("en-US").DateTimeFormat.GetMonthName(month),
            Results = results
        };

        return report;
    }
}