using System.ComponentModel.DataAnnotations.Schema;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity;
using RiverBooks.SharedKernel;
using RiverBooks.Users.Events;

namespace RiverBooks.Users.Domain;

public class ApplicationUser : IdentityUser, IHaveDomainEvents
{
    public string FullName { get; set; } = string.Empty;

    public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();

    private readonly List<CartItem> _cartItems = new();

    public IReadOnlyCollection<UserStreetAddress> Addresses => _addresses.AsReadOnly();

    private readonly List<UserStreetAddress> _addresses = new();

    
    #region Domain events

    private List<DomainEventBase> _domainEvents = new();
    [NotMapped] public IEnumerable<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

    protected void RegisterDomainEvent(DomainEventBase domainEvent) => _domainEvents.Add(domainEvent);
    void IHaveDomainEvents.ClearDomainEvents() => _domainEvents.Clear();

    #endregion

    
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
        
        RegisterDomainEvent(new AddressAddedEvent(newAddress));
        
        return newAddress;
    }

    #endregion
}