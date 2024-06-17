using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using RiverBooks.Books.Data;
using RiverBooks.Books.Services;
using RiverBooks.Books.Repositories;

namespace RiverBooks.Books;

public static class BookServiceExtensions
{
    public static IServiceCollection AddBookService(
        this IServiceCollection services,
        ConfigurationManager configuration,
        ILogger logger,
        List<Assembly> mediatRAssemblies)
    {
        services.AddDbContext<BookDbContext>(opts =>
            opts.UseSqlServer(configuration.GetConnectionString("BooksConnectionString")));
        services.AddScoped<IBookRepository, EfBookRepository>();
        
        services.AddScoped<IBookService, BookService>();
        
        mediatRAssemblies.Add(typeof(BookServiceExtensions).Assembly);
        logger.Information("{Module} module services registered", "Books");
        return services;
    }
}