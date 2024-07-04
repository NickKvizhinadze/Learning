using GymManagement.Domain.Subscriptions;
using GymManagement.TestCommon.TestConstants;

namespace GymManagement.TestCommon.Subscriptions;

public class SubscriptionFactory
{
    public static Subscription CreateSubscription(
        SubscriptionType? subscriptionType = null,
        Guid? adminId = null,
        Guid? id = null)
    {
        return new Subscription(
            subscriptionType: subscriptionType ?? Constants.Subscriptions.DefaultSubscriptionType,
            adminId: adminId ?? Constants.Admin.Id,
            id: id ?? Constants.Subscriptions.Id
        );
    }
}