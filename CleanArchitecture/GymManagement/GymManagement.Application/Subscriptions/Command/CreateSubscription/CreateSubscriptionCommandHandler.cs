using MediatR;
using ErrorOr;
using GymManagement.Application.Common;
using GymManagement.Domain.Subscriptions;

namespace GymManagement.Application.Subscriptions.Command.CreateSubscription;

public class CreateSubscriptionCommandHandler(
    IUnitOfWork unitOfWork,
    ISubscriptionsRepository subscriptionsRepository)
    : IRequestHandler<CreateSubscriptionCommand, ErrorOr<Subscription>>
{
    public async Task<ErrorOr<Subscription>> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var subscription = new Subscription(
            request.SubscriptionType,
            request.AdminId);
        
        await subscriptionsRepository.AddSubscriptionAsync(subscription);
        await unitOfWork.CommitChangesAsync();
        
        return subscription;
    }
}