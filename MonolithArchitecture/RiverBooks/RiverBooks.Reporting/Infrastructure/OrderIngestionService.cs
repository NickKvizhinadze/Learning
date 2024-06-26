using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RiverBooks.Reporting.Integrations;
using RiverBooks.Reporting.Interfaces;

namespace RiverBooks.Reporting.Infrastructure;

internal class OrderIngestionService(
    ILogger<OrderIngestionService> logger,
    IConfiguration configuration)
    : IOrderIngestionService
{
    private readonly string? _connectionString = configuration.GetConnectionString("ReportingConnectionString");
    private static bool _ensureTableCreated = false;

    public async Task AddOrUpdateMonthlyBookSalesAsync(BookSale sale)
    {
        if (!_ensureTableCreated)
            await CreateTableAsync();

        var sql = @"IF EXISTS (SELECT 1 FROM reporting.MonthlyBookSales WHERE BookId = @BookId AND Year = @Year AND Month = @Month)
                    BEGIN
                        -- Update existing record
                        UPDATE Reporting.MonthlyBookSales
                        SET UnitsSold = UnitsSold + @UnitsSold, TotalSales = TotalSales + @TotalSales
                        WHERE BookId = @BookId AND Year = @Year AND Month = @Month
                    END
                    ELSE
                    BEGIN
                        -- Insert new record
                        INSERT INTO reporting.MonthlyBookSales (BookId, Title, Author, Year, Month, UnitsSold, TotalSales)
                        VALUES (@BookId, @Title, @Author, @Year, @Month, @UnitsSold, @TotalSales)
                    END";
        await using var connection = new SqlConnection(_connectionString);
        logger.LogInformation("Executing query: {sql}", sql);
        await connection.ExecuteAsync(sql, new
        {
            sale.BookId,
            sale.Title,
            sale.Author,
            sale.Year,
            sale.Month,
            sale.UnitsSold,
            sale.TotalSales
        });
    }

    private async Task CreateTableAsync()
    {
        string sql = @"IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name='reporting')
                        BEGIN
                            EXEC('CREATE SCHEMA reporting');
                        END

                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name='MonthlyBookSales' AND type = 'U')
                        BEGIN
                            CREATE TABLE Reporting.MonthlyBookSales
                            (
                                BookId UNIQUEIDENTIFIER,
                                Author NVARCHAR(255),
                                Title NVARCHAR(255),
                                Year INT,
                                Month INT,
                                UnitsSold INT,
                                TotalSales DECIMAL(18, 2),
                                PRIMARY KEY (BookId, Month, Year)
                            );
                        END";

        await using var connection = new SqlConnection(_connectionString);
        logger.LogInformation("Executing query: {sql}", sql);
        await connection.ExecuteAsync(sql);
        _ensureTableCreated = true;
    }
}