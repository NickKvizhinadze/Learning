using Microsoft.AspNetCore.Mvc;

namespace RiverBooks.Reporting.ReportingEndpoints;

internal class TopSalesByMonthRequest
{
    [FromQuery]
    public int Year { get; init; }
    [FromQuery]
    public int Month { get; init; }
}