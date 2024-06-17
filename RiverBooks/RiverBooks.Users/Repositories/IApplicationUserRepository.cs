using RiverBooks.Users.Entities;

namespace RiverBooks.Users.Repositories;

internal interface IApplicationUserRepository
{
    Task<ApplicationUser?> GetUserWithCartByEmailAsync(string email);
    Task SaveChangesAsync();
}