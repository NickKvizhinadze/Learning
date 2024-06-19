using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RiverBooks.Users.Entities;

namespace RiverBooks.Users.Data.Configurations;

internal class UserStreetAddressConfiguration: IEntityTypeConfiguration<UserStreetAddress>
{
    public void Configure(EntityTypeBuilder<UserStreetAddress> builder)
    {
        builder.Property(item => item.Id)
            .ValueGeneratedNever();

        builder.ComplexProperty(item => item.StreetAddress);
    }
}