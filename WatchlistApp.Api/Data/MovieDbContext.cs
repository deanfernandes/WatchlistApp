using Microsoft.EntityFrameworkCore;
using WatchlistApp.Api.Models;

namespace WatchlistApp.Api.Data
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
    }
}
