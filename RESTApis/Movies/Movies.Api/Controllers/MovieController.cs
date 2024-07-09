using Microsoft.AspNetCore.Mvc;
using Movies.Api.Mapping;
using Movies.Contracts.Requests;
using Movies.Application.Repositories;

namespace Movies.Api.Controllers;

[ApiController]
public class MovieController : ControllerBase
{
    private readonly IMoviesRepository _moviesRepository;

    public MovieController(IMoviesRepository moviesRepository)
    {
        _moviesRepository = moviesRepository;
    }

    [HttpPost(ApiEndpoints.Movies.Create)]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
    {
        var movie = request.MapToMovie();
        await _moviesRepository.CreateAsync(movie);

        var response = movie.MapToResponse();

        return CreatedAtAction(nameof(Get), new { id = movie.Id }, response);
    }

    [HttpGet(ApiEndpoints.Movies.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var movie = await _moviesRepository.GetByIdAsync(id);
        if (movie is null)
            return NotFound();
        
        var response = movie.MapToResponse();
        return Ok(response);
    }

    [HttpGet(ApiEndpoints.Movies.GetAll)]
    public async Task<IActionResult> Get()
    {
        var movies = await _moviesRepository.GetAllAsync();
        
        var response = movies.MapToResponse();
        return Ok(response);
    }
    
    [HttpPut(ApiEndpoints.Movies.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request)
    {
        var movie = request.MapToMovie(id);
        var updated = await _moviesRepository.UpdateAsync(movie);

        if (!updated)
            return NotFound();
        
        var response = movie.MapToResponse();

        return Ok(response);
    }
    
    
    
    [HttpDelete(ApiEndpoints.Movies.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deleted = await _moviesRepository.DeleteByIdAsync(id);

        if (!deleted)
            return NotFound();
        
        return Ok();
    }
}