using MediatR;
using Microsoft.Extensions.Logging;
using RiverBooks.Books.Contracts;
using RiverBooks.OrderProcessing.Contracts;
using RiverBooks.Reporting.Interfaces;

namespace RiverBooks.Reporting.Integrations;

internal class NewOrderCreatedIngestionHandler(
    ILogger<NewOrderCreatedIngestionHandler> logger,
    IMediator mediator,
    IOrderIngestionService orderIngestionService)
    : INotificationHandler<OrderCreatedIntegrationEvent>
{
    public async Task Handle(OrderCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling order created event to populate reporting database");

        var orderItems = notification.OrderDetails.OrderItems;
        int year = notification.OrderDetails.DateCreated.Year;
        int month = notification.OrderDetails.DateCreated.Month;

        foreach (var item in orderItems)
        {
            // look ip book details to get author and title
            // TODO: implement materialized view or other cahce
            var bookDetailsQuery = new BookDetailsQuery(item.BookId);
            var result = await mediator.Send(bookDetailsQuery, cancellationToken);

            if (!result.IsSuccess)
            {
                logger.LogWarning("Issue loading book details for {id}", item.BookId);
                continue;
            }
            
            var author = result.Value.Author;
            var title = result.Value.Title;

            var sale = new BookSale
            {
                Author = author,
                BookId = item.BookId,
                Month = month,
                Year = year,
                Title = title,
                TotalSales = item.Quantity * item.UnitPrice,
                UnitsSold = item.Quantity
            };

            await orderIngestionService.AddOrUpdateMonthlyBookSalesAsync(sale);
        }
    }
}

internal class BookSale
{
    public string Author { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public Guid BookId { get; set; } = Guid.Empty;
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal TotalSales { get; set; }
    public int UnitsSold { get; set; }
}