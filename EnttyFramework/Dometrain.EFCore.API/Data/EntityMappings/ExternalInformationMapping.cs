using Dometrain.EFCore.API.Data.ValueConverters;
using Dometrain.EFCore.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dometrain.EFCore.API.Data.EntityMappings
{
    public class ExternalInformationMapping : IEntityTypeConfiguration<ExternalInformation>
    {
        public void Configure(EntityTypeBuilder<ExternalInformation> builder)
        {
            builder.HasKey(info => info.MovieId);

            builder.HasOne(info => info.Movie)
                .WithOne(movie => movie.ExternalInformation)
                .HasForeignKey<ExternalInformation>(info => info.MovieId)
                .HasPrincipalKey<Movie>(movie => movie.Identifier)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}