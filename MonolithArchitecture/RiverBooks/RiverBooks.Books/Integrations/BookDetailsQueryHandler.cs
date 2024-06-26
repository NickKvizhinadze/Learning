﻿using Ardalis.Result;
using MediatR;
using RiverBooks.Books.Contracts;
using RiverBooks.Books.Services;

namespace RiverBooks.Books.Integrations;

internal class BookDetailsQueryHandler(IBookService bookService)
    : IRequestHandler<BookDetailsQuery, Result<BookDetailsResponse>>
{
    public async Task<Result<BookDetailsResponse>> Handle(BookDetailsQuery request, CancellationToken cancellationToken)
    {
        var bookResult = await bookService.GetBookByIdAsync(request.BookId);

        if (bookResult.IsNotFound())
            return Result.NotFound();

        var book = bookResult.Value;
        var response = new BookDetailsResponse(book.Id, book.Title, book.Author, book.Price);

        return Result.Success(response);
    }
}