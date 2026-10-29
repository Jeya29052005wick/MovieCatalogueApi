
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCatalogueApi.Data;
using MovieCatalogueApi.Models;
using MovieCatalogueApi.Validators;

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

    private readonly MovieRequestValidator _validator;

    public MoviesController(
        ApplicationDbContext context,
        MovieRequestValidator validator)
    {
        _context = context;
        _validator = validator;
    }


    // GET: api/movies?search=leo&genre=Action
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMovies(
        [FromQuery] string? search,
        [FromQuery] string? genre)
    {
        IQueryable<Movie> query = _context.Movies.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.Trim();

            query = query.Where(m =>
                m.Title.ToLower().Contains(searchTerm.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(genre))
        {
            var genreFilter = genre.Trim();

            query = query.Where(m => m.Genre == genreFilter);
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
    public async Task<ActionResult<Movie>> CreateMovie(MovieRequest request)
    {
        var errors = _validator.Validate(request);

        if (errors.Count > 0)
        {
            return BadRequest(errors);
        }

        var movie = new Movie
        {
            Title = request.Title,
            Genre = request.Genre,
            Language = request.Language,
            DurationMinutes = request.DurationMinutes,
            ReleaseYear = request.ReleaseYear
        };

        _context.Movies.Add(movie);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetMovieById),
            new { id = movie.Id },
            movie);
    }



    // PUT: api/movies/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateMovie(
        int id,
        MovieRequest request)
    {
        var errors = _validator.Validate(request);

        if (errors.Count > 0)
        {
            return BadRequest(errors);
        }

        var existingMovie = await _context.Movies.FindAsync(id);

        if (existingMovie is null)
        {
            return NotFound();
        }

        existingMovie.Title = request.Title;
        existingMovie.Genre = request.Genre;
        existingMovie.Language = request.Language;
        existingMovie.DurationMinutes = request.DurationMinutes;
        existingMovie.ReleaseYear = request.ReleaseYear;

        await _context.SaveChangesAsync();

        return NoContent();
    }




    // DELETE: api/movies/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        var movie = await _context.Movies.FindAsync(id);

        if (movie is null)
        {
            return NotFound();
        }

        _context.Movies.Remove(movie);

        await _context.SaveChangesAsync();

        return NoContent();
    }


}
