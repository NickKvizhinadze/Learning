using Microsoft.EntityFrameworkCore;
using RiverBooks.Users.Data;
using RiverBooks.Users.Entities;

namespace RiverBooks.Users.Repositories;

internal class EfApplicationUserRepository(UsersDbContext context) : IApplicationUserRepository
{
    public Task<ApplicationUser?> GetUserWithCartByEmailAsync(string email)
    {
        return context.ApplicationUser
            .Include(c => c.CartItems)
            .SingleOrDefaultAsync(u => u.Email == email);
    }

    public Task SaveChangesAsync()
    {
        return context.SaveChangesAsync();
    }
}