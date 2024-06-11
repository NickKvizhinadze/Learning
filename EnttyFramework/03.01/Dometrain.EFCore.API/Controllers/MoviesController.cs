using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dometrain.EFCore.API.Data;
using Dometrain.EFCore.API.Models;

namespace Dometrain.EFCore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController : Controller
{
    private readonly MoviesContext _context;

    public MoviesController(MoviesContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<Movie>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _context.Movies.ToListAsync());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var movie = await _context.Movies
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
        var filteredTitles = await _context.Movies
            .Where(m => m.ReleaseDate.Year == year)
            .Select(m => new MovieTitle {Id = m.Identifier, Title = m.Title})
            .ToListAsync();

        return Ok(filteredTitles);
    }


    [HttpGet("until-age/{ageRating}")]
    [ProducesResponseType(typeof(List<MovieTitle>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByAgeRating([FromRoute] AgeRating ageRating)
    {
        var filteredTitles = await _context.Movies
            .Where(m => m.AgeRating <= ageRating)
            .Select(m => new MovieTitle {Id = m.Identifier, Title = m.Title})
            .ToListAsync();

        return Ok(filteredTitles);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Movie), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] Movie movie)
    {
        await _context.Movies.AddAsync(movie);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = movie.Identifier }, movie);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Movie movie)
    {
        var existingMovie = await _context.Movies.FindAsync(id);

        if (existingMovie is null)
            return NotFound();

        existingMovie.Title = movie.Title;
        existingMovie.Synopsis = movie.Synopsis;
        existingMovie.ReleaseDate = movie.ReleaseDate;

        await _context.SaveChangesAsync();

        return Ok(existingMovie);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove([FromRoute] int id)
    {
        var existingMovie = await _context.Movies.FindAsync(id);

        if (existingMovie is null)
            return NotFound();

        _context.Movies.Remove(existingMovie);
        await _context.SaveChangesAsync();

        return Ok();
    }
}
