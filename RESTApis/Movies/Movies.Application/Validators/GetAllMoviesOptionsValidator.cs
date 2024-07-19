using System.Data;
using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Validators;

public class GetAllMoviesOptionsValidator: AbstractValidator<GetAllMoviesOptions>
{
    private readonly string[] _acceptableSortFields = ["title", "yearofrelease"];
    public GetAllMoviesOptionsValidator()
    {
        RuleFor(m => m.Year)
            .LessThanOrEqualTo(DateTime.UtcNow.Year);

        RuleFor(m => m.SortField)
            .Must(field => field is null || _acceptableSortFields.Contains(field, StringComparer.OrdinalIgnoreCase))
            .WithMessage("You can only sort by title or yearofrelease");

        RuleFor(m => m.Page)
            .GreaterThan(1);
        
        RuleFor(m => m.PageSize)
            .InclusiveBetween(1, 25)
            .WithMessage("You can get between 1 to 25 movies per page");
    }
}