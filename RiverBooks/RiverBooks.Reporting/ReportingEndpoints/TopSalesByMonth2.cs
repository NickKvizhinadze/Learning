using FastEndpoints;
using RiverBooks.Reporting.Interfaces;

namespace RiverBooks.Reporting.ReportingEndpoints;

internal class TopSalesByMonth2(ISalesReportingService salesReportingService)
    : Endpoint<TopSalesByMonthRequest, TopSalesByMonthResponse>
{
    public override void Configure()
    {
        Get("/topsales2");
        AllowAnonymous(); //TODO: lock down
    }

    public override async Task HandleAsync(TopSalesByMonthRequest req, CancellationToken cancellationToken)
    {
        var report = await salesReportingService.GetTopBooksByMonthReportAsync(req.Year, req.Month);
        await SendOkAsync(new TopSalesByMonthResponse(report), cancellation: cancellationToken);
    }
}