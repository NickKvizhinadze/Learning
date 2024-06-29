using GymManagement.Application.Common;
using GymManagement.Domain.Subscriptions;
using GymManagement.Infrastructure.Common.Persistence;

namespace GymManagement.Infrastructure.Subscriptions.Persistence;

public class SubscriptionsRepository(GymManagementDbContext dbContext) : ISubscriptionsRepository
{
    public async Task AddSubscriptionAsync(Subscription subscription)
    {
        await dbContext.Subscriptions.AddAsync(subscription);
    }

    public async Task<Subscription?> GetSubscriptionAsync(Guid id)
    {
        var subscription = await dbContext.Subscriptions.FindAsync(id);
        return subscription;
    }
}