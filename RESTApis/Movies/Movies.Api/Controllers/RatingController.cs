using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Contracts.Requests;
using Movies.Application.Repositories;
using Movies.Application.Services;

namespace Movies.Api.Controllers;

[ApiController]
public class RatingController(IRatingService _ratingService) : ControllerBase
{
    [Authorize]
    [HttpPut(ApiEndpoints.Movies.Rate)]
    public async Task<IActionResult> Create([FromRoute] Guid id, [FromBody] RateMovieRequest request,
        CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var result = await _ratingService.RateMovieAsync(id, request.Rating, userId!.Value, token);

        return result ? Ok() : NotFound();
    }
    
    [Authorize]
    [HttpDelete(ApiEndpoints.Movies.DeleteRating)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var result = await _ratingService.DeleteRatingAsync(id, userId!.Value, token);

        return result ? Ok() : NotFound();
    }
    
    [Authorize]
    [HttpGet(ApiEndpoints.Ratings.GetUserRatings)]
    public async Task<IActionResult> GetUserRatings(CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var ratings = await _ratingService.GetRatingsForUserAsync(userId!.Value, token);

        var ratingsResponse = ratings.MapToResponse();
        return Ok(ratingsResponse);
    }
}