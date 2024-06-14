using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dometrain.EFCore.API.Data;
using Dometrain.EFCore.API.Models;

namespace Dometrain.EFCore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController(MoviesContext context) : Controller
{
    [HttpGet]
    [ProducesResponseType(typeof(List<Movie>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var movies = await context.Movies
            // .Include(movie => movie.Actors) // include all actors for all movies
            .AsNoTracking()
            .ToListAsync();
        
        // Load actors only for television movies
        foreach (var televisionMovie in movies.OfType<TelevisionMovie>())
        {
            await context.Entry(televisionMovie)
                .Collection(movie => movie.Actors)
                .LoadAsync();
        }
        
        return Ok(movies);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var movie = await context.Movies
            .Include(movie => movie.Genre)
            .SingleOrDefaultAsync(movie => movie.Identifier == id);

        return movie is null
            ? NotFound()
            : Ok(movie);
    }

    [HttpGet("by-year/{year:int}")]
    [ProducesResponseType(typeof(List<MovieTitle>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByYear([FromRoute] int year)
    {
        var filteredTitles = await context.Movies
            .Where(m => m.ReleaseDate.Year == year)
            .Select(m => new MovieTitle {Id = m.Identifier, Title = m.Title})
            .ToListAsync();

        return Ok(filteredTitles);
    }

    private static readonly Func<MoviesContext, AgeRating, IEnumerable<MovieTitle>>? _compiledQuery = EF.CompileQuery((MoviesContext context, AgeRating ageRating) =>
        context.Movies
            .Where(m => m.AgeRating <= ageRating)
            .Select(m => new MovieTitle {Id = m.Identifier, Title = m.Title}));
    
    [HttpGet("until-age/{ageRating}")]
    [ProducesResponseType(typeof(List<MovieTitle>), StatusCodes.Status200OK)]
    public IActionResult GetByAgeRating([FromRoute] AgeRating ageRating)
    {
        var filteredTitles = _compiledQuery!(context, ageRating).ToList();

        return Ok(filteredTitles);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Movie), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] Movie movie)
    {
        await context.Movies.AddAsync(movie);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = movie.Identifier }, movie);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Movie movie)
    {
        var existingMovie = await context.Movies.FindAsync(id);

        if (existingMovie is null)
            return NotFound();

        existingMovie.Title = movie.Title;
        existingMovie.Synopsis = movie.Synopsis;
        existingMovie.ReleaseDate = movie.ReleaseDate;

        await context.SaveChangesAsync();

        return Ok(existingMovie);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove([FromRoute] int id)
    {
        var existingMovie = await context.Movies.FindAsync(id);

        if (existingMovie is null)
            return NotFound();

        context.Movies.Remove(existingMovie);
        await context.SaveChangesAsync();

        return Ok();
    }
}
