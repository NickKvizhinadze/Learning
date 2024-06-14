using Microsoft.AspNetCore.Mvc;
using Dometrain.EFCore.API.Models;
using Dometrain.EfCore.API.Repositories;
using Dometrain.EFCore.API.Services;

namespace Dometrain.EFCore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class GenresController(IGenreRepository repository, IBatchGenreService batchGenreService) : Controller
{
    [HttpGet]
    [ProducesResponseType(typeof(List<Genre>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await repository.GetAll());
    }

    [HttpGet("from-query")]
    [ProducesResponseType(typeof(IEnumerable<Genre>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllFromQuery()
    {
        return Ok(await repository.GetAllFromQuery());
    }

    [HttpGet("names")]
    [ProducesResponseType(typeof(IEnumerable<GenreName>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNames()
    {
        return Ok(await repository.GetNames());
    }
    
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Genre), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var genre = await repository.Get(id);

        return genre is null
            ? NotFound()
            : Ok(genre);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Genre), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] Genre genre)
    {
        genre = await repository.Create(genre);

        return CreatedAtAction(nameof(Get), new { id = genre.Id }, genre);
    }

    [HttpPost("batch")]
    [ProducesResponseType(typeof(Genre), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAll([FromBody] List<Genre> genres)
    {
        var response = await batchGenreService.CreateGenres(genres);

        return CreatedAtAction(nameof(GetAll), response);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Genre), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Genre genre)
    {
        var updatedGenre = await repository.Update(id, genre);

        return Ok(updatedGenre);
    }
    
    [HttpPut("batch-update")]
    [ProducesResponseType(typeof(Genre), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAll([FromBody] List<Genre> genres)
    {
        var response = await batchGenreService.UpdateGenres(genres);

        return CreatedAtAction(nameof(GetAll), new {}, response);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove([FromRoute] int id)
    {
        await repository.Delete(id);
        return Ok();
    }
}