using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RiverBooks.Books.Entities;

namespace RiverBooks.Books.Data.Configurations;

internal class BookConfiguration: IEntityTypeConfiguration<Book>
{
    internal static readonly Guid Book1Guid = new ("476d9cf4-4c88-4341-9fbe-eef8703e40e7");
    internal static readonly Guid Book2Guid = new ("1f41c855-1300-404f-8735-47fae437966d");
    internal static readonly Guid Book3Guid = new ("5f23f2df-434c-4383-aae7-04652678d4dc");

    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.Property(b => b.Title)
            .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);
        
        builder.Property(b => b.Author)
            .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);

        builder.HasData(GetBookSampleData());
    }
    
    private static Book[] GetBookSampleData()
    {
        var tolkien = "J.R.R. Tolkien";
        return
        [
            new Book(Book1Guid, "The Fellowship of the Ring", tolkien, 10.99m),
            new Book(Book2Guid, "The Two Towers", tolkien, 11.99m),
            new Book(Book3Guid, "The Return of the King", tolkien, 12.99m)
        ];
    }
}