using GymManagement.Application.Common;
using GymManagement.Domain.Subscriptions;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Common.Persistence;

public class GymManagementDbContext(DbContextOptions<GymManagementDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Subscription> Subscriptions { get; set; } = null!;
    
    
    public async Task CommitChangesAsync()
    {
        await base.SaveChangesAsync();
    }
}