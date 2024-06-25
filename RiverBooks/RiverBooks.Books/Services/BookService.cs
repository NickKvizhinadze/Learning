using Ardalis.Result;
using Microsoft.Extensions.Logging;
using RiverBooks.Books.Models;
using RiverBooks.Books.Entities;
using RiverBooks.Books.Repositories;

namespace RiverBooks.Books.Services;

internal class BookService(
    IBookRepository bookRepository,
    ILogger<BookService> logger) : IBookService
{
    public async Task<List<BookDto>> ListBooksAsync()
    {
        var books = (await bookRepository.ListAsync())
            .Select(b => new BookDto(b.Id, b.Title, b.Author, b.Price))
            .ToList();

        return books;
    }

    public async Task<Result<BookDto>> GetBookByIdAsync(Guid id)
    {
        var book = await bookRepository.GetByIdAsync(id);

        if (book is not null)
            return Result.Success(new BookDto(book.Id, book.Title, book.Author, book.Price));

        logger.LogError("Book with id: {id} not found", id);
        return Result<BookDto>.NotFound();
    }

    public async Task CreateBookAsync(BookDto newBook)
    {
        var book = new Book(newBook.Id, newBook.Title, newBook.Author, newBook.Price);

        await bookRepository.AddAsync(book);
        await bookRepository.SaveChangesAsync();
    }

    public async Task<Result> DeleteBookAsync(Guid id)
    {
        var book = await bookRepository.GetByIdAsync(id);
        if (book is not null)
        {
            await bookRepository.DeleteAsync(book);
            await bookRepository.SaveChangesAsync();
            return Result.Success();
        }

        logger.LogError("Book with id: {id} not found", id);
        return Result.NotFound();
    }

    public async Task<Result> UpdateBookPriceAsync(Guid id, decimal newPrice)
    {
        try
        {
            var book = await bookRepository.GetByIdAsync(id);
            if (book is not null)
            {
                book.UpdatePrice(newPrice);
                await bookRepository.SaveChangesAsync();
                return Result.Success();
            }

            logger.LogError("Book with id: {id} not found", id);
            return Result.NotFound();
        }
        catch (Exception e)
        {
            logger.LogError("Book price could not update for book id: {id}, with price {}. Error: {error}", id,
                newPrice, e.Message);
            return Result.Error("Internal server error");
        }
    }
}