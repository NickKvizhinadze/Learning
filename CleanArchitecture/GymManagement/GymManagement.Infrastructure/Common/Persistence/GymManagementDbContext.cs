using System.Reflection;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Admins;
using GymManagement.Domain.Common;
using GymManagement.Domain.Gyms;
using GymManagement.Domain.Subscriptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Common.Persistence;

public class GymManagementDbContext : DbContext, IUnitOfWork
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DbSet<Admin> Admins { get; set; } = null!;
    public DbSet<Subscription> Subscriptions { get; set; } = null!;
    public DbSet<Gym> Gyms { get; set; } = null!;

    public GymManagementDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task CommitChangesAsync()
    {
        await SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = new CancellationToken())
    {
        int result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(e => e.Entity.PopDomainEvents())
            .SelectMany(e => e)
            .ToList();

        AddDomainEventsToOfflineProcess(domainEvents);
        return result;
    }

    private void AddDomainEventsToOfflineProcess(List<IDomainEvent> domainEvents)
    {
        var domainEventsQueue =
            _httpContextAccessor.HttpContext!.Items.TryGetValue("DomainEventsQueue", out var value) &&
            value is Queue<IDomainEvent> existingQueue
                ? existingQueue
                : new Queue<IDomainEvent>();
        
        domainEvents.ForEach(domainEventsQueue.Enqueue);
        _httpContextAccessor.HttpContext!.Items["DomainEventsQueue"] = domainEventsQueue;
    }
}