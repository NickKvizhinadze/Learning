namespace Movies.Application.Models;

public class GetAllMoviesOptions
{
    public Guid? UserId { get; set; }
    public required string? Title { get; set; }
    public required int? Year { get; set; }
}