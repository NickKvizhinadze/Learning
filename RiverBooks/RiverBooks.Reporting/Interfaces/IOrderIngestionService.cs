using RiverBooks.Reporting.Integrations;

namespace RiverBooks.Reporting.Interfaces;

internal interface IOrderIngestionService
{
    Task AddOrUpdateMonthlyBookSalesAsync(BookSale sale);
}