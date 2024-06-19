﻿namespace RiverBooks.Users.Entities;

public class UserStreetAddress
{
    public UserStreetAddress(string userId, Address streetAddress)
    {
        UserId = userId;
        StreetAddress = streetAddress;
    }

    private UserStreetAddress()
    {
        // Required by EF Core
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string UserId { get; private set; } = string.Empty;
    public Address StreetAddress { get; private set; } = default!;
}