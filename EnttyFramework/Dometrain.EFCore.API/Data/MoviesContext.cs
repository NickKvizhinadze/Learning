using Microsoft.EntityFrameworkCore;
using Dometrain.EFCore.API.Models;
using Dometrain.EFCore.API.Data.EntityMappings;
using Dometrain.EFCore.API.Data.Interceptors;

namespace Dometrain.EFCore.API.Data
{
    public class MoviesContext : DbContext
    {
        public MoviesContext(DbContextOptions<MoviesContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies => Set<Movie>();
        public DbSet<ExternalInformation> ExternalInformations => Set<ExternalInformation>();
        public DbSet<Actor> Actors => Set<Actor>();
        public DbSet<Genre> Genres => Set<Genre>();
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GenreMapping());
            modelBuilder.ApplyConfiguration(new MovieMapping());
            modelBuilder.ApplyConfiguration(new CinemaMovieMapping());
            modelBuilder.ApplyConfiguration(new TelevisionMovieMapping());
            modelBuilder.ApplyConfiguration(new ExternalInformationMapping());
            modelBuilder.ApplyConfiguration(new ActorsMapping());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new SaveChangesInterceptor());
        }
    }
}
