using RiverBooks.Reporting.Domain;

namespace RiverBooks.Reporting.Interfaces;

internal interface ISalesReportingService
{
    Task<TopBooksByMonthReport> GetTopBooksByMonthReportAsync(int year, int month);
}