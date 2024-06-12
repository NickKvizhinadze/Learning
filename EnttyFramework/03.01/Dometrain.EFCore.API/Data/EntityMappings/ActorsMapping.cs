using Dometrain.EFCore.API.Data.ValueConverters;
using Dometrain.EFCore.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dometrain.EFCore.API.Data.EntityMappings
{
    public class ActorsMapping : IEntityTypeConfiguration<Actor>
    {
        public void Configure(EntityTypeBuilder<Actor> builder)
        {
            builder
                .HasMany(actor => actor.Movies)
                .WithMany(movie => movie.Actors)
                .UsingEntity("Movie_Actor",
                    left => left
                                        .HasOne(typeof(Movie))
                                        .WithMany()
                                        .HasForeignKey("MovieId")
                                        .HasPrincipalKey(nameof(Movie.Identifier))
                                        .OnDelete(DeleteBehavior.Cascade),
                    right => right
                                        .HasOne(typeof(Actor))
                                        .WithMany()
                                        .HasForeignKey("ActorId")
                                        .HasPrincipalKey(nameof(Actor.Id))
                                        .OnDelete(DeleteBehavior.Cascade),
                    linkBuilder => linkBuilder.HasKey("MovieId", "ActorId"));
        }
    }
}