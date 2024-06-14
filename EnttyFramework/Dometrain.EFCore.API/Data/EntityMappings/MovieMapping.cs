using Dometrain.EFCore.API.Data.ValueConverters;
using Dometrain.EFCore.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dometrain.EFCore.API.Data.EntityMappings
{
    public class MovieMapping : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.UseTphMappingStrategy()
                .HasQueryFilter(movie => movie.ReleaseDate >= new DateTime(1990, 1, 1))
                .HasKey(movie => movie.Identifier);

            builder.HasAlternateKey(movie => new { movie.Title, movie.ReleaseDate });

            builder.HasIndex(movie => movie.AgeRating)
                .IsDescending();
            
            builder.Property(movie => movie.Title)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();
        
            builder.Property(movie => movie.ReleaseDate)
                .HasColumnType("char(8)")
                .HasConversion(new DateTimeToChar8Converter());
        
            builder.Property(movie => movie.Synopsis)
                .HasColumnType("varchar(max)")
                .HasColumnName("Plot");
        
            builder.Property(movie => movie.AgeRating)
                .HasColumnType("char(32)")
                .HasConversion<string>();
            
            builder.Property(movie => movie.MainGenreName)
                .HasColumnType("varchar")
                .HasMaxLength(256);
            
            builder
                .HasOne(movie => movie.Genre)
                .WithMany(genre => genre.Movies)
                .HasPrincipalKey(genre => genre.Name)
                .HasForeignKey(movie => movie.MainGenreName);
        
            //Seeding the data
            // builder.HasData(new Movie
            // {
            //     Identifier = 1,
            //     Title = "Fight Club",
            //     ReleaseDate = new DateTime(1999, 09, 10),
            //     Synopsis = "Ed Norton and Brad Pitt have a couple of fist fights with each other.",
            //     MainGenreId = 1,
            //     AgeRating = AgeRating.Adolescent
            // });
        }
    }
    
    public class CinemaMovieMapping : IEntityTypeConfiguration<CinemaMovie>
    {
        public void Configure(EntityTypeBuilder<CinemaMovie> builder)
        {
        }
    }

    public class TelevisionMovieMapping : IEntityTypeConfiguration<TelevisionMovie>
    {
        public void Configure(EntityTypeBuilder<TelevisionMovie> builder)
        {
        }
    }
}
