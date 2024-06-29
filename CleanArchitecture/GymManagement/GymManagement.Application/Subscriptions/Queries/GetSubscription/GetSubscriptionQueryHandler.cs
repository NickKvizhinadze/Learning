using GymManagement.Application.Common;
using GymManagement.Domain.Subscriptions;
using MediatR;
using ErrorOr;

namespace GymManagement.Application.Subscriptions.Queries.GetSubscription;

public class GetSubscriptionQueryHandler(
    ISubscriptionsRepository subscriptionsRepository
) : IRequestHandler<GetSubscriptionQuery, ErrorOr<Subscription>>
{
    public async Task<ErrorOr<Subscription>> Handle(GetSubscriptionQuery request, CancellationToken cancellationToken)
    {
        var subscription = await subscriptionsRepository.GetSubscriptionAsync(request.SubscriptionId);
        return subscription is null 
            ? Error.NotFound(description: "Subscription not found.") 
            : subscription;
    }
}