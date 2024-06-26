namespace RiverBooks.Users.Models;

public record CartItemDto(Guid BookId, string Title, string Description, int Quantity, decimal UnitPrice);