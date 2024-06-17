using RiverBooks.Users.Models;

namespace RiverBooks.Users.CartEndpoints;

public class CartResponse(List<CartItemDto> cartItems)
{
    public List<CartItemDto> CartItems { get; set; } = cartItems;
}