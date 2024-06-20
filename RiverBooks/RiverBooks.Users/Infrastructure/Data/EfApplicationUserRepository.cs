using Microsoft.EntityFrameworkCore;
using RiverBooks.Users.Domain;
using RiverBooks.Users.Interfaces;

namespace RiverBooks.Users.Infrastructure.Data;

internal class EfApplicationUserRepository(UsersDbContext context) : IApplicationUserRepository
{
    public Task<ApplicationUser?> GetUserWithCartByEmailAsync(string email)
    {
        return context.ApplicationUser
            .Include(c => c.CartItems)
            .SingleOrDefaultAsync(u => u.Email == email);
    }

    public Task<ApplicationUser?> GetUserWithAddressesByEmailAsync(string email)
    {
        return context.ApplicationUser
            .Include(c => c.Addresses)
            .SingleOrDefaultAsync(u => u.Email == email);
    }

    public Task SaveChangesAsync()
    {
        return context.SaveChangesAsync();
    }
}