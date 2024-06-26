using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RiverBooks.OrderProcessing.Domain;

namespace RiverBooks.OrderProcessing.Infrastructure.Data.Configurations;

internal class OrderConfiguration: IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(item => item.Id)
            .ValueGeneratedNever();

        builder.ComplexProperty(o => o.ShippingAddress, address =>
        {
            address.Property(a => a.Street1)
                .HasMaxLength(DataSchemaConstants.STREET_MAXLENGTH);
            address.Property(a => a.Street2)
                .HasMaxLength(DataSchemaConstants.STREET_MAXLENGTH);
            address.Property(a => a.City)
                .HasMaxLength(DataSchemaConstants.CITY_MAXLENGTH);
            address.Property(a => a.State)
                .HasMaxLength(DataSchemaConstants.STATE_MAXLENGTH);
            address.Property(a => a.PostalCode)
                .HasMaxLength(DataSchemaConstants.POSTALCODE_MAXLENGTH);
            address.Property(a => a.Country)
                .HasMaxLength(DataSchemaConstants.COUNTRY_MAXLENGTH);
        });
        
        builder.ComplexProperty(o => o.BillingAddress, address =>
        {
            address.Property(a => a.Street1)
                .HasMaxLength(DataSchemaConstants.STREET_MAXLENGTH);
            address.Property(a => a.Street2)
                .HasMaxLength(DataSchemaConstants.STREET_MAXLENGTH);
            address.Property(a => a.City)
                .HasMaxLength(DataSchemaConstants.CITY_MAXLENGTH);
            address.Property(a => a.State)
                .HasMaxLength(DataSchemaConstants.STATE_MAXLENGTH);
            address.Property(a => a.PostalCode)
                .HasMaxLength(DataSchemaConstants.POSTALCODE_MAXLENGTH);
            address.Property(a => a.Country)
                .HasMaxLength(DataSchemaConstants.COUNTRY_MAXLENGTH);
        });
    }
}

internal class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(item => item.Id)
            .ValueGeneratedNever();

        builder.Property(item => item.Description)
            .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);
    }
}