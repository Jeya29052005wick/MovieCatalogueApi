
using MovieCatalogueApi.Models;

namespace MovieCatalogueApi.Validators;

public class MovieRequestValidator
{
    public List<string> Validate(MovieRequest request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.Title) ||
            request.Title.Length > 200)
        {
            errors.Add("Title is required and must not exceed 200 characters.");
        }

        if (string.IsNullOrWhiteSpace(request.Genre) ||
            request.Genre.Length > 100)
        {
            errors.Add("Genre is required and must not exceed 100 characters.");
        }

        if (string.IsNullOrWhiteSpace(request.Language) ||
            request.Language.Length > 100)
        {
            errors.Add("Language is required and must not exceed 100 characters.");
        }

        if (request.DurationMinutes < 1 ||
            request.DurationMinutes > 600)
        {
            errors.Add("Duration must be between 1 and 600 minutes.");
        }

        if (request.ReleaseYear < 1888 ||
            request.ReleaseYear > 2100)
        {
            errors.Add("Release year must be between 1888 and 2100.");
        }

        return errors;
    }
}
