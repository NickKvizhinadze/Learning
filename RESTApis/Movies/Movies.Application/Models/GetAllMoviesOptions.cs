namespace Movies.Application.Models;

public class GetAllMoviesOptions
{
    public Guid? UserId { get; set; }
    public string? Title { get; set; }
    public int? Year { get; set; }
    public string? SortField { get; set; }
    public SortOrder? SortOrder { get; set; }
    public required int PageSize { get; set; }
    public required int Page { get; set; }
}

public enum SortOrder
{
    Unsorted,
    Ascending,
    Descending
}