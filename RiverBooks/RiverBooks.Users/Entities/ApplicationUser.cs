using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity;

namespace RiverBooks.Users.Entities;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;

    public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();

    private readonly List<CartItem> _cartItems = new();

    public IReadOnlyCollection<UserStreetAddress> Addresses => _addresses.AsReadOnly();

    private readonly List<UserStreetAddress> _addresses = new();

    #region Methods

    internal void AddToCart(CartItem item)
    {
        Guard.Against.Null(_cartItems);

        var existingBook = _cartItems.SingleOrDefault(c => c.BookId == item.BookId);
        if (existingBook != null)
        {
            existingBook.UpdateQuantity(existingBook.Quantity + item.Quantity);
            existingBook.UpdateDescription(item.Description);
            existingBook.UpdateUnitPrice(item.UnitPrice);
            return;
        }

        _cartItems.Add(item);
    }

    internal void ClearCart()
    {
        _cartItems.Clear();
    }

    internal UserStreetAddress AddAddress(Address address)
    {
        Guard.Against.Null(address);

        var existingAddress = _addresses.SingleOrDefault(a => a.StreetAddress == address);
        if (existingAddress is not null)
            return existingAddress;

        var newAddress = new UserStreetAddress(Id, address);

        _addresses.Add(newAddress);
        return newAddress;
    }

    #endregion
}