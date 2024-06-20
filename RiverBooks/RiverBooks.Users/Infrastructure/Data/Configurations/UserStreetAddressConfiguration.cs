using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RiverBooks.Users.Domain;

namespace RiverBooks.Users.Infrastructure.Data.Configurations;

internal class UserStreetAddressConfiguration: IEntityTypeConfiguration<UserStreetAddress>
{
    public void Configure(EntityTypeBuilder<UserStreetAddress> builder)
    {
        builder.ToTable("UserStreetAddress");
        builder.Property(item => item.Id)
            .ValueGeneratedNever();

        builder.ComplexProperty(item => item.StreetAddress);
    }
}