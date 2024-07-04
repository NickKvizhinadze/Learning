using GymManagement.Application.Gyms.Commands.CreateGym;
using GymManagement.Application.Subscriptions.Commands.CreateSubscription;
using GymManagement.Domain.Subscriptions;
using GymManagement.TestCommon.TestConstants;

namespace GymManagement.TestCommon.Subscriptions;

public static class SubscriptionCommandFactory
{
    public static CreateSubscriptionCommand CreateSubscriptionCommandFactory(
        SubscriptionType? subscriptionType = null,
        Guid? adminId = null)
    {
        return new CreateSubscriptionCommand(
            SubscriptionType: subscriptionType ?? Constants.Subscriptions.DefaultSubscriptionType,
            AdminId: adminId ?? Constants.Admin.Id);
    }
}