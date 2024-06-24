using System.Globalization;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RiverBooks.Reporting.Domain;
using RiverBooks.Reporting.Interfaces;

namespace RiverBooks.Reporting.Infrastructure;

internal class TopSellingBooksReportServiceService : ITopSellingBooksReportService
{
    private readonly string _connectionString;
    private readonly ILogger<TopSellingBooksReportServiceService> _logger;

    public TopSellingBooksReportServiceService(IConfiguration config,
        ILogger<TopSellingBooksReportServiceService> logger)
    {
        _connectionString = config.GetConnectionString("OrderProcessingConnectionString")!;
        _logger = logger;
    }

    public TopBooksByMonthReport ReachInSqlQuery(int year, int month)
    {
        string sql =
            @"select b.Id, b.Title, b.Author, SUM(oi.Quantity) as Units, SUM(oi.UnitPrice * oi.Quantity) as Sales
                        from books.Books b
                        inner join orderprocessing.OrderItem oi ON oi.bookId = b.Id
                        inner join orderprocessing.Orders o ON o.Id = oi.OrderId
                        where Month(o.DateCreated) = @month
                        and YEAR(o.DateCreated) = @year
                        group by b.Id, b.Title, b.Author
                        order by Sales desc";

        using var connection = new SqlConnection(_connectionString);
        _logger.LogInformation("Executing query: {sql}", sql);

        var results = connection
            .Query<BookSalesResult>(sql, new { month, year })
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