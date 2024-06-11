using Microsoft.EntityFrameworkCore;
using Dometrain.EFCore.API.Models;
using Dometrain.EFCore.API.Data.EntityMappings;

namespace Dometrain.EFCore.API.Data
{
    public class MoviesContext : DbContext
    {
        public MoviesContext(DbContextOptions<MoviesContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies => Set<Movie>();
        public DbSet<Genre> Genres => Set<Genre>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GenreMapping());
            modelBuilder.ApplyConfiguration(new MovieMapping());
        }
    }
}
