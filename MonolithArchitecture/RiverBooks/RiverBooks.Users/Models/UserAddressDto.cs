namespace RiverBooks.Users.Models;

public record UserAddressDto(
    string UserId,
    string Street1,
    string Street2,
    string City,
    string State,
    string PostalCode,
    string Country);