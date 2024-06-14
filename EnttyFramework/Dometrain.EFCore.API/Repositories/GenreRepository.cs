using Dometrain.EFCore.API.Data;
using Dometrain.EFCore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Dometrain.EfCore.API.Repositories;

public interface IGenreRepository
{
    Task<IEnumerable<Genre>> GetAll();
    Task<IEnumerable<Genre>> GetAll(IEnumerable<int> ids);
    Task<Genre?> Get(int id);
    Task<Genre> Create(Genre genre);
    Task<Genre?> Update(int id, Genre genre);
    Task<bool> Delete(int id);
    Task<List<Genre>> GetAllFromQuery();
    Task<List<GenreName>> GetNames();
}

public class GenreRepository(MoviesContext context, IUnitOfWorkManager unitOfWorkManager)
    : IGenreRepository
{
    public async Task<IEnumerable<Genre>> GetAll()
    {
        return await context.Genres.ToListAsync();
    }
    
    public async Task<IEnumerable<Genre>> GetAll(IEnumerable<int> ids)
    {
        return await context.Genres.Where(genre => ids.Contains(genre.Id)).ToListAsync();
    }

    public async Task<Genre?> Get(int id)
    {
        return await context.Genres.FindAsync(id);
    }

    public async Task<Genre> Create(Genre genre)
    {
        await context.Genres.AddAsync(genre);
        
        if(!unitOfWorkManager.IsUnitOfWorkStarted)
            await context.SaveChangesAsync();

        return genre;
    }

    public async Task<Genre?> Update(int id, Genre genre)
    {
        var existingGenre = await context.Genres.FindAsync(id);

        if (existingGenre is null)
            return null;

        existingGenre.Name = genre.Name;
        existingGenre.Description = genre.Description;

        if(!unitOfWorkManager.IsUnitOfWorkStarted)
            await context.SaveChangesAsync();

        return existingGenre;
    }

    public async Task<bool> Delete(int id)
    {
        var existingGenre = await context.Genres.FindAsync(id);

        if (existingGenre is null)
            return false;

        context.Genres.Remove(existingGenre);

        if(!unitOfWorkManager.IsUnitOfWorkStarted)
            await context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Genre>> GetAllFromQuery()
    {
        var minimumGenreId = 2;
        
        var genres = await context.Genres
            .FromSql($"SELECT * FROM Genres WHERE Id > {minimumGenreId}")
            // .FromSqlRaw("SELECT * FROM Genres WHERE Id > {0}", minimumGenreId)
            .Where(genre => genre.Name != "Comedy")
            .ToListAsync();

        return genres;
    }

    public async Task<List<GenreName>> GetNames()
    {
        var names = await context
            .Database
            .SqlQuery<GenreName>($"SELECT Name FROM Genres")
            .ToListAsync();
        return names;
    }
}