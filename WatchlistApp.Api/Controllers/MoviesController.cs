using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WatchlistApp.Api.Data;
using WatchlistApp.Api.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WatchlistApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieDbContext _context;

        public MoviesController(MovieDbContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies([FromQuery] string? genre, [FromQuery] bool? watched)
        {
            var moviesQuery = _context.Movies.AsQueryable();

            if (!string.IsNullOrEmpty(genre))
            {
                moviesQuery = moviesQuery.Where(m => m.Genre == genre);
            }

            if (watched.HasValue)
            {
                moviesQuery = moviesQuery.Where(m => m.Watched == watched.Value);
            }

            var movies = await moviesQuery.ToListAsync();
            return Ok(movies);
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, [FromBody] MovieDTO movieDTO)
        {
            var existingMovie = await _context.Movies.FindAsync(id);

            if (existingMovie == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(movieDTO.Title))
            {
                existingMovie.Title = movieDTO.Title;
            }
            existingMovie.Watched = movieDTO.Watched;
            existingMovie.Genre = movieDTO.Genre;

            _context.Entry(existingMovie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(MovieDTO movieDTO)
        {
            if (movieDTO == null || string.IsNullOrWhiteSpace(movieDTO.Title))
            {
                return BadRequest("Movie title cannot be empty.");
            }

            Movie movie = new Movie()
            {
                Title = movieDTO.Title,
                Watched = movieDTO.Watched,
                Genre = movieDTO.Genre,
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movieDTO);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
