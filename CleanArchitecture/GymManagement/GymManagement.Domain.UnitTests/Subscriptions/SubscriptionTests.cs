using ErrorOr;
using FluentAssertions;
using GymManagement.Domain.Subscriptions;
using GymManagement.TestCommon.Gyms;
using GymManagement.TestCommon.Subscriptions;
namespace GymManagement.Domain.UnitTests.Subscriptions;

public class SubscriptionTests
{
    [Fact]
    public void AddGym_WhenMoreThanSubscriptionAllows_ShouldFail()
    {
        // Arrange
        // Create a subscription
        var subscription = SubscriptionFactory.CreateSubscription();
        // Create the maxim number of gym + 1
        var gyms = Enumerable.Range(0, subscription.GetMaxGyms() + 1)
            .Select(_ => GymFactory.CreateGym(id: Guid.NewGuid()))
            .ToList();
        // Act
        var addGymResults = gyms.ConvertAll(subscription.AddGym);
        
        // Add all the various gyms

        var allButLastGymResults = addGymResults[..^1];

        allButLastGymResults.Should()
            .AllSatisfy(addGymResult => addGymResult.Value.Should().Be(Result.Success));

        var lastGymResult = addGymResults.Last();
        lastGymResult.IsError.Should().BeTrue();
        lastGymResult.FirstError.Should().Be(SubscriptionErrors.CannotHaveMoreGymsThanSubscriptionAllows);
    }
}