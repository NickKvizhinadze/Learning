using RiverBooks.Reporting.Domain;

namespace RiverBooks.Reporting.Interfaces;

internal interface ITopSellingBooksReportService
{
    TopBooksByMonthReport ReachInSqlQuery(int year, int month);
}