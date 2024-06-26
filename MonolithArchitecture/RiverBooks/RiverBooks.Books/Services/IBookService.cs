using Ardalis.Result;
using RiverBooks.Books.Models;

namespace RiverBooks.Books.Services;

internal interface IBookService
{
    Task<List<BookDto>> ListBooksAsync();
    Task<Result<BookDto>> GetBookByIdAsync(Guid id);
    Task CreateBookAsync(BookDto newBook);
    Task<Result> DeleteBookAsync(Guid id);
    Task<Result> UpdateBookPriceAsync(Guid id, decimal newPrice);
}