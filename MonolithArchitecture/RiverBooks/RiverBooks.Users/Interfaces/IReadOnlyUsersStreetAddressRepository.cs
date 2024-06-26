using RiverBooks.Users.Domain;

namespace RiverBooks.Users.Interfaces;

internal interface IReadOnlyUsersStreetAddressRepository
{
    Task<UserStreetAddress?> GetByIdAsync(Guid id);
}