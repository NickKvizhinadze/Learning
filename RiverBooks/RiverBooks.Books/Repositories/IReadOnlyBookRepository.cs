using RiverBooks.Books.Entities;

namespace RiverBooks.Books.Repositories;

internal interface IReadOnlyBookRepository
{
    Task<Book?> GetByIdAsync(Guid id);
    Task<List<Book>> ListAsync();
}