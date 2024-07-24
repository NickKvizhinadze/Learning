using Microsoft.AspNetCore.OutputCaching;
using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Application.Repositories;
using Movies.Application.Services;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Endpoints.Movies;

public static class UpdateMovieEndpoint
{
    public const string Name = "UpdateMovie";

    public static IEndpointRouteBuilder MapUpdateMovie(this IEndpointRouteBuilder app)
    {
        app.MapPut(ApiEndpoints.Movies.Update, async (
                Guid id, 
                UpdateMovieRequest request,
                IMovieService movieService,
                HttpContext context,
                IOutputCacheStore outputCacheStore,
                CancellationToken token) =>
        {
            var user = context.GetUserId();
            var movie = request.MapToMovie(id);
            var updatedMovie = await movieService.UpdateAsync(movie, user, token);

            if (updatedMovie is null)
                return Results.NotFound();

            var response = movie.MapToResponse();

            await outputCacheStore.EvictByTagAsync("movies", token);
            return TypedResults.Ok(response);
        })
            .WithName(Name)
            .Produces<MovieResponse>(StatusCodes.Status200OK)
            .Produces<ValidationFailureResponse>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(AuthConstants.TrustedMemberPolicyName);

        return app;
    }
}