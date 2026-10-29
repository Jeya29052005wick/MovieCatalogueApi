
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCatalogueApi.Data;
using MovieCatalogueApi.Models;

namespace MovieCatalogueApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MoviesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/movies
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMovies(
        [FromQuery] string? search,
        [FromQuery] string? genre)
    {
        IQueryable<Movie> query = _context.Movies.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(m => m.Title.Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(genre))
        {
            query = query.Where(m => m.Genre == genre);
        }

        var movies = await query.ToListAsync();

        return Ok(movies);
    }

    // GET: api/movies/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Movie>> GetMovieById(int id)
    {
        var movie = await _context.Movies
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movie is null)
        {
            return NotFound();
        }

        return Ok(movie);
    }


    // POST: api/movies
    [HttpPost]
    public async Task<ActionResult<Movie>> CreateMovie(Movie movie)
    {
        _context.Movies.Add(movie);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetMovieById),
            new { id = movie.Id },
            movie);
    }


    // PUT: api/movies/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateMovie(int id, Movie updatedMovie)
    {
        if (id != updatedMovie.Id)
        {
            return BadRequest("The URL ID does not match the movie ID.");
        }

        var existingMovie = await _context.Movies.FindAsync(id);

        if (existingMovie is null)
        {
            return NotFound();
        }

        existingMovie.Title = updatedMovie.Title;
        existingMovie.Genre = updatedMovie.Genre;
        existingMovie.Language = updatedMovie.Language;
        existingMovie.DurationMinutes = updatedMovie.DurationMinutes;
        existingMovie.ReleaseYear = updatedMovie.ReleaseYear;

        await _context.SaveChangesAsync();

        return NoContent();
    }


}
