using FastEndpoints;

namespace RiverBooks.Books;

internal class ListBookEndpoints(IBookService bookService) : EndpointWithoutRequest<ListBookResponses>
{
    public override void Configure()
    {
        Get("/books");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var books = bookService.ListBooks();

        await SendAsync(new ListBookResponses
        {
            Books = books
        }, cancellation: cancellationToken);
    }
}