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
    private readonly IPublisher _publisher;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DbSet<Admin> Admins { get; set; } = null!;
    public DbSet<Subscription> Subscriptions { get; set; } = null!;
    public DbSet<Gym> Gyms { get; set; } = null!;

    public GymManagementDbContext(
        DbContextOptions options,
        IHttpContextAccessor httpContextAccessor,
        IPublisher publisher) : base(options)
    {
        _publisher = publisher;
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
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(e => e.Entity.PopDomainEvents())
            .SelectMany(e => e)
            .ToList();

        if (IsUserWaitingOnline())
            AddDomainEventsToOfflineProcess(domainEvents);
        else
        {
            await PublishDomainEvents(cancellationToken, domainEvents);
        }

        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private bool IsUserWaitingOnline() => _httpContextAccessor.HttpContext is not null;
    
    private async Task PublishDomainEvents(CancellationToken cancellationToken, List<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
            await _publisher.Publish(domainEvent, cancellationToken);
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