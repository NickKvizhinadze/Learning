using GymManagement.Application.Gyms.Commands.CreateGym;
using GymManagement.TestCommon.TestConstants;

namespace GymManagement.TestCommon.Gyms;

public static class GymCommandFactory
{
    public static CreateGymCommand CreateCreateGymCommand(
        string name = Constants.Gym.Name,
        Guid? subscriptionId = null)
    {
        return new CreateGymCommand(
            Name: name,
            SubscriptionId: subscriptionId ?? Constants.Subscriptions.Id);
    } 
}