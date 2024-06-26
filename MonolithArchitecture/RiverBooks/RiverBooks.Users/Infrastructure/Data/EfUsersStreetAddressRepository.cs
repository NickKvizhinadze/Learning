using Microsoft.EntityFrameworkCore;
using RiverBooks.Users.Domain;
using RiverBooks.Users.Interfaces;

namespace RiverBooks.Users.Infrastructure.Data;

internal class EfUsersStreetAddressRepository(UsersDbContext context)
    : IReadOnlyUsersStreetAddressRepository
{
    public Task<UserStreetAddress?> GetByIdAsync(Guid id) =>
        context.UserStreetAddresses
            .SingleOrDefaultAsync(a => a.Id == id);
}