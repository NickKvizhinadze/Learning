using FastEndpoints;
using RiverBooks.Reporting.Interfaces;

namespace RiverBooks.Reporting.ReportingEndpoints;

internal class TopSalesByMonth(ITopSellingBooksReportService topSellingBooksReportService)
    : Endpoint<TopSalesByMonthRequest, TopSalesByMonthResponse>
{
    public override void Configure()
    {
        Get("/topsales");
        AllowAnonymous(); //TODO: lock down
    }

    public override async Task HandleAsync(TopSalesByMonthRequest req, CancellationToken cancellationToken)
    {
        var report = topSellingBooksReportService.ReachInSqlQuery(req.Year, req.Month);
        await SendOkAsync(new TopSalesByMonthResponse(report), cancellation: cancellationToken);
    }
}