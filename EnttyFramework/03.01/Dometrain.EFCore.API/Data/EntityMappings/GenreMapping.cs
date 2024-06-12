using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dometrain.EFCore.API.Models;
using Dometrain.EFCore.API.Data.ValueGenerators;

namespace Dometrain.EFCore.API.Data.EntityMappings
{
    public class GenreMapping : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.Property<DateTime>("CreatedDate")
                .HasColumnName("CreatedAt")
                //.HasDefaultValueSql("GETDATE()");
                .HasValueGenerator<CreatedDateGenerator>();

            builder.Property(genre => genre.Name)
                .HasColumnType("varchar")
                .HasMaxLength(256);


            builder
                .Property<bool>("Deleted")
                .HasDefaultValue(false);

            builder.HasQueryFilter(genre => EF.Property<bool>(genre, "Deleted") == false)
                .HasAlternateKey(genre => genre.Name);


            builder.HasData(new Genre { Id = 1, Name = "Drama" });
        }
    }
}