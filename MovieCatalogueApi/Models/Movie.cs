
using System.ComponentModel.DataAnnotations;

namespace MovieCatalogueApi.Models;

public class Movie
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Genre { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Language { get; set; } = string.Empty;

    [Range(1, 600)]
    public int DurationMinutes { get; set; }

    [Range(1888, 2100)]
    public int ReleaseYear { get; set; }
}
